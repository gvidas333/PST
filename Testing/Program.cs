namespace Selenium;

public class Program
{
    static void Main(string[] args)
    {
        //TestOne.RunTestOne();
        //TestTwo.RunTestTwo();
        //TestThree.RunTestThree();
        TestFour testFour = new TestFour();
        testFour.Setup();
        testFour.TestScenario1();
        testFour.TestScenario2();
        testFour.Teardown();
    }
}