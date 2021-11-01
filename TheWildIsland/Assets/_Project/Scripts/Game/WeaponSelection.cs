using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class WeaponSelection : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public Weapons weaponType;
        public WeaponBalancer weaponBalancer;
    }

    [SerializeField] private List<Weapon> _selectionWeapons = new List<Weapon>();
    [SerializeField] private GameObject _weaponsStats;
    [SerializeField] private Slider _attackStatSlider;
    [SerializeField] private Slider _blastRadiusStatSlider;
    [SerializeField] private Slider _costStatSlider;

    private Weapon _selectedWeapon;

    public void SelectWeapon(int weapon)
    {
        _selectedWeapon = _selectionWeapons[weapon];
    }

    public void ShowWeaponStats(int weapon)
    {
        _weaponsStats.SetActive(true);
        _blastRadiusStatSlider.value = _selectionWeapons[weapon].weaponBalancer.blastRadius;
        _attackStatSlider.value = _selectionWeapons[weapon].weaponBalancer.damage;
        _costStatSlider.value = _selectionWeapons[weapon].weaponBalancer.cost;
    }

    public void HideWeaponStats(int weapon)
    {
        _weaponsStats.SetActive(false);
    }
}
