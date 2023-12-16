namespace JworkzResonitePowerShellModule.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var ex = new Example();
            var car = new Car();

            ex.Fire += car.SendFire;
            ex.Print();
        }
    }
}