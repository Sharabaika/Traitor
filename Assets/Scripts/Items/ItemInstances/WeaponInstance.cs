using System.Runtime.InteropServices;
using Characters;
using Items.ScriptableItems;
using Misc;
using Photon.Chat;
using ScriptableItems;
using UnityEngine;

namespace Items.ItemInstances
{
    public class WeaponInstance : ItemInstance
    {
        public int RemainingAmmo { get; private set; } = 0;
        private WeaponData _data;

        public WeaponInstance(ItemData data,int remainingAmmo): base(data)
        {
            RemainingAmmo = remainingAmmo;
            _data =(WeaponData) data;
        }

        public bool WasteAmmo()
        {
            if (RemainingAmmo > 0)
            {
                RemainingAmmo--;
                return true;
            }

            return false;
        }

        public void Reload(PlayerCharacter character)
        {
            var missingAmmo = _data.AmmoCapacity - RemainingAmmo;
            var inventory = character.Inventory;
            
            for (int i = 0; i < missingAmmo; i++)
            {
                if (inventory.RemoveOneOf(_data.AmmoTypes))
                {
                    RemainingAmmo++;
                }
                else
                {
                    break;
                }
            }
        }


        public override string GetStatus()
        {
            return $"{RemainingAmmo}\\{_data.AmmoCapacity}";
        }

        public override string SerializeState()
        {
            return RemainingAmmo.ToString();
        }

        public override void DeserializeState(string data)
        {
            var values = data.Split(':');
            RemainingAmmo = int.Parse(values[0]);
        }
    }
}