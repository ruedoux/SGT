// Testing a testing framework with the same testing framework for testing
namespace PassingTests;
using System;
using SGT;


public class AssertionTest : SimpleTestClass
{

  [SimpleTestMethod]
  public void TestAssertTrue()
  {
    // Pass case
    Assertions.AssertTrue(true);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertTrue(false);
    });
  }

  [SimpleTestMethod]
  public void TestAssertFalse()
  {
    // Pass case
    Assertions.AssertFalse(false);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertFalse(true);
    });
  }

  [SimpleTestMethod]
  public void TestAssertEqual()
  {
    // Pass case
    Assertions.AssertEqual(2, 2);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertEqual(2, 3);
    });
  }

  [SimpleTestMethod]
  public void TestAssertNotEqual()
  {
    // Pass case
    Assertions.AssertNotEqual(2, 3);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertNotEqual(2, 2);
    });
  }

  [SimpleTestMethod]
  public void TestAssertEqualOrLessThan()
  {
    // Pass case
    Assertions.AssertEqualOrLessThan(1, 1);
    Assertions.AssertEqualOrLessThan(1, 0);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertEqualOrLessThan(1, 2);
    });
  }

  [SimpleTestMethod]
  public void TestAssertEqualOrMoreThan()
  {
    // Pass case
    Assertions.AssertEqualOrMoreThan(1, 1);
    Assertions.AssertEqualOrMoreThan(1, 2);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertEqualOrMoreThan(1, 0);
    });
  }

  [SimpleTestMethod]
  public void TestAssertLessThan()
  {
    // Pass case
    Assertions.AssertLessThan(1, 0);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertLessThan(1, 1);
      Assertions.AssertLessThan(1, 2);
    });
  }

  [SimpleTestMethod]
  public void TestAssertMoreThan()
  {
    // Pass case
    Assertions.AssertMoreThan(1, 2);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertMoreThan(1, 1);
      Assertions.AssertMoreThan(1, 0);
    });
  }

  [SimpleTestMethod]
  public void TestAssertInRange()
  {
    // Pass case
    Assertions.AssertInRange(0, 2, 0);
    Assertions.AssertInRange(0, 2, 1);
    Assertions.AssertInRange(0, 2, 2);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertInRange(0, 2, 3);
      Assertions.AssertInRange(0, 2, -1);
    });
  }

  [SimpleTestMethod]
  public void TestAssertNotInRange()
  {
    // Pass case
    Assertions.AssertNotInRange(0, 2, 3);
    Assertions.AssertNotInRange(0, 2, -1);

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertNotInRange(0, 2, 0);
      Assertions.AssertNotInRange(0, 2, 1);
      Assertions.AssertNotInRange(0, 2, 2);
    });
  }

  [SimpleTestMethod]
  public void TestAssertThrows()
  {
    // Pass case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      throw new AssertionException("test");
    });

    // Fail case
    Assertions.AssertThrows<AssertionException>(() =>
    {
      Assertions.AssertThrows<AssertionException>(() =>
      {
        throw new Exception("test");
      });

      Assertions.AssertThrows<AssertionException>(() => { });
    });
  }
}
