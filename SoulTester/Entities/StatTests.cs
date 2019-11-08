using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoulTester.Entities
{
    using SoulCollector.Entities;

    [TestClass]
    public class StatTests
    {
        private Stat _testableStat;
        [TestInitialize]
        public void SetUp()
        {
            _testableStat = new Stat(StatType.Defense, 100);
        }

        [TestMethod]
        public void CanConvertToInt()
        {
            int result = _testableStat;
            int expected = _testableStat.Value;
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void CanAddFlatModifier()
        {
            int added = 10;
            int expected = _testableStat.Value + added;
            StatModifier mod = new StatModifier(added);
            _testableStat.AddModifier(mod);
            
            Assert.AreEqual(expected, _testableStat.Value);
        }

        [TestMethod]
        public void CanAddMultipleFlatModifiers()
        {
            int added = 10;
            int expected = _testableStat.Value + added * 2;
            StatModifier mod = new StatModifier(added);
            _testableStat.AddModifier(mod);
            _testableStat.AddModifier(mod);
           
            Assert.AreEqual(expected, _testableStat.Value);
        }

        [TestMethod]
        public void CanRemoveFlatModifier()
        {
            int added = 10;
            int beforeAdd = _testableStat.Value;
            int expected = _testableStat.Value + added;

            StatModifier mod = new StatModifier(added);

            _testableStat.AddModifier(mod);
            Assert.AreEqual(expected, _testableStat.Value);

            _testableStat.RemoveModifier(mod);
            Assert.AreEqual(beforeAdd, _testableStat.Value);
        }

        [TestMethod]
        public void CanAddPercentageModifier()
        {
            float added = 0.1f;
            int expected = _testableStat.Value + (int)(added * _testableStat.Base);
            StatModifier mod = new StatModifier(added);
            _testableStat.AddModifier(mod);
            
            Assert.AreEqual(expected, _testableStat.Value);
        }

        [TestMethod]
        public void CanAddMultiplePercentageModifier()
        {
            float added = 0.1f;
            int expected = _testableStat.Value + (int)(added * 2 * _testableStat.Base);
            StatModifier mod = new StatModifier(added);
            _testableStat.AddModifier(mod);
            _testableStat.AddModifier(mod);
            
            Assert.AreEqual(expected, _testableStat.Value);
        }

        [TestMethod]
        public void CanRemovePercentageModifier()
        {
            float added = 0.1f;
            int beforeAdd = _testableStat.Value;
            int expected = _testableStat.Value + (int)(added * _testableStat.Base);
            StatModifier mod = new StatModifier(added);

            _testableStat.AddModifier(mod);
            Assert.AreEqual(expected, _testableStat.Value);

            _testableStat.RemoveModifier(mod);
            Assert.AreEqual(beforeAdd, _testableStat.Value);
        }

        [TestMethod]
        public void CanAddMixedModifiers()
        {
            float added = 0.1f;
            int flatAdded = 20;
            int expected = _testableStat.Value + (int)(added * _testableStat.Base) + flatAdded;
            StatModifier mod = new StatModifier(added);
            StatModifier mod2 = new StatModifier(flatAdded);
            _testableStat.AddModifier(mod);
            _testableStat.AddModifier(mod2);

            Assert.AreEqual(expected, _testableStat.Value);
        }


        [TestMethod]
        public void CanRemoveMixedModifiers()
        {
            float added = 0.1f;
            
            int flatAdded = 20;
            int expected = _testableStat.Value + (int)(added * _testableStat.Base) + flatAdded;
            int beforePercentag = _testableStat.Value + flatAdded;
            StatModifier mod = new StatModifier(added);
            StatModifier mod2 = new StatModifier(flatAdded);

            _testableStat.AddModifier(mod);
            _testableStat.AddModifier(mod2);
            Assert.AreEqual(expected, _testableStat.Value);

            _testableStat.RemoveModifier(mod);
            Assert.AreEqual(beforePercentag, _testableStat.Value);


        }
    }
}
