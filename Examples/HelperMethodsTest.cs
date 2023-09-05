namespace PassingTests;
using SGT;


public class HelperMethods : SimpleTestClass
{
  private int shouldBeZero = 0;
  private bool shouldBeFalse = false;


  [SimpleBeforeEach]
  public void BeforeEach()
  {
    shouldBeZero = 0;
  }

  [SimpleAfterEach]
  public void AfterEach()
  {
    shouldBeFalse = false;
  }

  [SimpleTestMethod(repeatTest = 2)]
  public void TestBeforeEach()
  {
    shouldBeZero++;
    Assertions.AssertEqual(shouldBeZero, 1);
  }


  [SimpleTestMethod(repeatTest = 2)]
  public void TestAfterEach()
  {
    Assertions.AssertFalse(shouldBeFalse);
    shouldBeFalse = true;
  }
}