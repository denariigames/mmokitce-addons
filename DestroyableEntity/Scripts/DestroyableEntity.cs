/**
 * DestroyableEntity
 * Author: Denarii Games
 * Version: 1.0
 */

using UnityEngine;
using System.Collections.Generic;
using Insthync.UnityEditorUtils;

namespace MultiplayerARPG
{
	public partial class DestroyableEntity : HarvestableEntity
	{
		[Category(6, "Detroyable Settings")]
		[SerializeField] protected bool useItemsFromFirstWeapon = true;

		protected override void ApplyReceiveDamage(HitBoxPosition position, Vector3 fromPosition, EntityInfo instigator, Dictionary<DamageElement, MinMaxFloat> damageAmounts, CharacterItem weapon, BaseSkill skill, int skillLevel, int randomSeed, out CombatAmountType combatAmountType, out int totalDamage)
		{
			instigator.TryGetEntity(out BaseCharacterEntity attackerCharacter);
			// Apply damages, won't apply skill damage
			float calculatingTotalDamage = 0f;
			// Harvest type is based on weapon by default
			HarvestType skillHarvestType = HarvestType.BasedOnWeapon;
			if (skill != null && skillLevel > 0)
			{
				skillHarvestType = skill.HarvestType;
			}
			// Get random damage
			switch (skillHarvestType)
			{
				case HarvestType.BasedOnWeapon:
					{
						IWeaponItem weaponItem = weapon.GetWeaponItem();
						HarvestEffectiveness harvestEffectiveness;
						if (harvestable.CacheHarvestEffectivenesses.TryGetValue(weaponItem.WeaponType, out harvestEffectiveness))
							calculatingTotalDamage = weaponItem.HarvestDamageAmount.GetAmount(weapon.level).Random(randomSeed) * harvestEffectiveness.damageEffectiveness;
					}
					break;
				case HarvestType.BasedOnSkill:
					{
						SkillHarvestEffectiveness skillHarvestEffectiveness;
						if (harvestable.CacheSkillHarvestEffectivenesses.TryGetValue(skill, out skillHarvestEffectiveness))
							calculatingTotalDamage = skill.HarvestDamageAmount.GetAmount(skillLevel).Random(randomSeed) * skillHarvestEffectiveness.damageEffectiveness;
					}
					break;
			}

			if (attackerCharacter != null)
				attackerCharacter.RewardExp((int)(harvestable.expPerDamage * calculatingTotalDamage), 1, RewardGivenType.Harvestable, 1, 1);

			// Apply damages
			combatAmountType = CombatAmountType.NormalDamage;
			totalDamage = CurrentGameInstance.GameplayRule.GetTotalDamage(fromPosition, instigator, this, calculatingTotalDamage, weapon, skill, skillLevel);
			if (totalDamage < 0)
				totalDamage = 0;
			CurrentHp -= totalDamage;
		}

		public override void ReceivedDamage(HitBoxPosition position, Vector3 fromPosition, EntityInfo instigator, Dictionary<DamageElement, MinMaxFloat> damageAmounts, CombatAmountType combatAmountType, int totalDamage, CharacterItem weapon, BaseSkill skill, int skillLevel, CharacterBuff buff, bool isDamageOverTime = false)
		{
			base.ReceivedDamage(position, fromPosition, instigator, damageAmounts, combatAmountType, totalDamage, weapon, skill, skillLevel, buff, isDamageOverTime);
			instigator.TryGetEntity(out BaseCharacterEntity attackerCharacter);
			CurrentGameInstance.GameplayRule.OnHarvestableReceivedDamage(attackerCharacter, this, combatAmountType, totalDamage, weapon, skill, skillLevel, buff, isDamageOverTime);

			if (combatAmountType == CombatAmountType.Miss)
				return;

			// Do something when entity dead
			if (this.IsDead())
			{
				// Get randomizer and random damage
				WeightedRandomizer<ItemDropForHarvestable> itemRandomizer = null;

				//new ItemDrop logic to use first weapon
				if (useItemsFromFirstWeapon && harvestable.harvestEffectivenesses.Length > 0 && harvestable.harvestEffectivenesses[0].weaponType != null)
				{
					harvestable.CacheHarvestItems.TryGetValue(harvestable.harvestEffectivenesses[0].weaponType, out itemRandomizer);
				}
				else
				{
					HarvestType skillHarvestType = HarvestType.BasedOnWeapon;
					if (skill != null && skillLevel > 0)
					{
						skillHarvestType = skill.HarvestType;
					}
					// Get randomizer and random damage
					if (skillHarvestType != HarvestType.None)
					{
						if (skillHarvestType == HarvestType.BasedOnWeapon)
						{
							IWeaponItem weaponItem = weapon.GetWeaponItem();
							harvestable.CacheHarvestItems.TryGetValue(weaponItem.WeaponType, out itemRandomizer);
						}
						else
							harvestable.CacheSkillHarvestItems.TryGetValue(skill, out itemRandomizer);
					}
				}

				// If found randomizer, random dropping items
				if (itemRandomizer != null)
				{
					ItemDropForHarvestable receivingItem = itemRandomizer.TakeOne();
					int itemDataId = receivingItem.item.DataId;
					int itemAmount = (int)receivingItem.amountPerDamage;
					bool droppingToGround = collectType == HarvestableCollectType.DropToGround;

					if (attackerCharacter != null)
					{
						if (attackerCharacter.IncreasingItemsWillOverwhelming(itemDataId, itemAmount))
							droppingToGround = true;
						if (!droppingToGround)
						{
							GameInstance.ServerGameMessageHandlers.NotifyRewardItem(attackerCharacter.ConnectionId, RewardGivenType.Harvestable, itemDataId, itemAmount);
							attackerCharacter.IncreaseItems(CharacterItem.Create(itemDataId, 1, itemAmount));
							attackerCharacter.FillEmptySlots();
						}
					}
					else
					{
						// Attacker is not character, always drop item to ground
						droppingToGround = true;
					}

					if (droppingToGround)
						ItemDropEntity.Drop(this, RewardGivenType.Harvestable, CharacterItem.Create(itemDataId, 1, itemAmount), new string[0]);
				}

				DestroyAndRespawn();
			}
		}
	}
}