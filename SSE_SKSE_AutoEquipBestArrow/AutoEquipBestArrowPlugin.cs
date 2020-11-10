using Microsoft.Win32;
using NetScriptFramework;
using NetScriptFramework.SkyrimSE;
using NetScriptFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetScriptFramework.SkyrimSE.ExtraContainerChanges;

namespace SSE_SKSE_AutoEquipBestArrow
{
    class AutoEquipBestArrowPlugin : Plugin
	{
		public override string Key
		{
			get
			{
				return "autoequipbestarrow";
			}
		}

		public override string Name
		{
			get
			{
				return "AutoEquipBestArrow Plugin";
			}
		}

		public override int Version
		{
			get
			{
				return 1;
			}
		}

		protected override bool Initialize(bool loadedAny)
		{
			Events.OnSpendAmmo.Register(OnSpendAmmoHandler);
			return true;
		}

		private void OnSpendAmmoHandler(SpendAmmoEventArgs e)
		{
			//MenuManager.ShowHUDMessage("Event OK", null, true);

			var actor = e.Spender;

			if (actor == null || (!actor.IsPlayer && !actor.IsPlayerTeammate))
			{
				//MenuManager.ShowHUDMessage("Not the player", null, true);
				return;
            }

			//MenuManager.ShowHUDMessage("Is the player", null, true);

			TESAmmo bestAmmo = null;
			ItemEntry ammoItem = null;
			
			foreach (var item in actor.Inventory.Objects)
			{
				if (item.Template.FormType != FormTypes.Ammo)
				{
					continue;
				}
				
				TESAmmo ammo = item.Template as TESAmmo;

				//MenuManager.ShowHUDMessage("Arrow found has " + ammo.AmmoData.Damage + " damage", null, true);

				if (bestAmmo == null || bestAmmo.AmmoData.Damage < ammo.AmmoData.Damage)
				{
					bestAmmo = ammo;
					ammoItem = item;
				}
			}

			//MenuManager.ShowHUDMessage("Best Arrow has " + (bestAmmo != null ? bestAmmo.AmmoData.Damage.ToString() : "???") + " damage", null, true);
			
			actor.EquipItem(ammoItem.Template, false, true);

			MenuManager.ShowHUDMessage("Equipping arrow " + ammoItem.Template.Name, null, true);
		}
	}
}
