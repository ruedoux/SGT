namespace PassingTests;
using SGT;

[SimpleTestClass]
public class ExampleHelperMethods
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

  [SimpleTestMethod]
  public void TestBeforeEach1()
  {
    shouldBeZero++;
    Assertions.AssertEqual(shouldBeZero, 1);
  }

  [SimpleTestMethod]
  public void TestBeforeEach2()
  {
    shouldBeZero++;
    Assertions.AssertEqual(shouldBeZero, 1);
  }

  [SimpleTestMethod]
  public void TestAfterEach1()
  {
    Assertions.AssertFalse(shouldBeFalse);
    shouldBeFalse = true;
  }

  [SimpleTestMethod]
  public void TestAfterEach2()
  {
    Assertions.AssertFalse(shouldBeFalse);
    shouldBeFalse = true;
  }
}