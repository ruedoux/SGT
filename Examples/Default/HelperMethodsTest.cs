namespace DefaultTests;
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

  public void TestBeforeEach()
  {
    shouldBeZero++;
    Assertions.AssertEqual(1, shouldBeZero);
  }

  public void TestAfterEach()
  {
    Assertions.AssertFalse(shouldBeFalse);
    shouldBeFalse = true;
  }
}