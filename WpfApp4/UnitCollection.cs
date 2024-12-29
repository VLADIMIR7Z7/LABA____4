using System.Collections.Generic;

namespace WizardChessUI
{
    public class UnitCollection
    {
        public List<Unit> Units { get; set; }

        public UnitCollection()
        {
            Units = new List<Unit>();
        }

        public void Add(Unit unit)
        {
            Units.Add(unit);
        }

        public bool Contains(Unit unit)
        {
            return Units.Contains(unit);
        }

        public List<Unit> GetAliveUnits()
        {
            return Units.FindAll(unit => unit.IsAlive);
        }
    }
}