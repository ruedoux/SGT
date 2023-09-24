namespace SGT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class Assertions
{
  public static void AssertTrue(bool shoudlBeTrue, string additionalMessage = "")
  {
    if (!shoudlBeTrue)
    {
      throw new AssertionException(
        $"Value is false, but expected true. {additionalMessage}");
    }
  }

  public static void AssertFalse(bool shoudlBeFalse, string additionalMessage = "")
  {
    if (shoudlBeFalse)
    {
      throw new AssertionException(
        $"Value is true, but expected false. {additionalMessage}");
    }
  }

  public static void AssertEqual<T>(
    T shouldBe, T isNow, string additionalMessage = "")
  {
    if (!Equals(shouldBe, isNow))
    {
      throw new AssertionException(
        $"Value is not equal, is: '{isNow}', but should be: '{shouldBe}'." + additionalMessage);
    }
  }

  public static void AssertEqual<T>(
    IEnumerable<T> shouldBe, IEnumerable<T> isNow, string additionalMessage = "")
  {
    if (!Equals(shouldBe, isNow))
    {
      throw new AssertionException(
        $"Value is not equal, is: '{isNow}', but should be: '{shouldBe}'." + additionalMessage);
    }
  }

  public static void AssertNotEqual<T>(
    T shouldNotBe, T isNow, string additionalMessage = "")
  {
    if (Equals(shouldNotBe, isNow))
    {
      throw new AssertionException(
        $"Value is equal to: '{shouldNotBe}'. {additionalMessage}");
    }
  }

  public static void AssertLessThan<T>(
    T maxValue, T value, string additionalMessage = "")
        where T : IComparable<T>
  {
    if (!(value.CompareTo(maxValue) < 0))
    {
      throw new AssertionException(
          $"Value '{value}' is not lesser than '{maxValue}'. {additionalMessage}");
    }
  }

  public static void AssertMoreThan<T>(
    T minValue, T value, string additionalMessage = "")
      where T : IComparable<T>
  {
    if (!(value.CompareTo(minValue) > 0))
    {
      throw new AssertionException(
          $"Value '{value}' is not larger than '{minValue}'. {additionalMessage}");
    }
  }

  public static void AssertEqualOrLessThan<T>(
    T maxValue, T value, string additionalMessage = "")
        where T : IComparable<T>
  {
    if (!(value.CompareTo(maxValue) <= 0))
    {
      throw new AssertionException(
          $"Value '{value}' is greater than '{maxValue}'. {additionalMessage}");
    }
  }

  public static void AssertEqualOrMoreThan<T>(
    T minValue, T value, string additionalMessage = "")
      where T : IComparable<T>
  {
    if (!(value.CompareTo(minValue) >= 0))
    {
      throw new AssertionException(
          $"Value '{value}' is less than '{minValue}'. {additionalMessage}");
    }
  }

  public static void AssertNotInRange<T>(
    T minValue, T maxValue, T value, string additionalMessage = "")
      where T : IComparable<T>
  {
    if (value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0)
    {
      throw new AssertionException(
          $"Value '{value}' is in range: '{minValue}' - '{maxValue}'. {additionalMessage}");
    }
  }

  public static void AssertInRange<T>(
    T minValue, T maxValue, T value, string additionalMessage = "")
      where T : IComparable<T>
  {
    if (value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0)
    {
      throw new AssertionException(
          $"Value '{value}' is not in range: '{minValue}' - '{maxValue}'. {additionalMessage}");
    }
  }

  public static void AssertThrows<T>(
    Action action, string additionalMessage = "") where T : Exception
  {
    try
    {
      action();
    }
    catch (T)
    {
      return;
    }
    catch (Exception ex)
    {
      throw new AssertionException(
        $"Expected {typeof(T)}, but got {ex.GetType()} instead. {additionalMessage}");
    }

    throw new AssertionException($"Expected {typeof(T)} was not thrown. {additionalMessage}");
  }

  public static void AssertAwaitAtMost(long timeoutMs, Action action, string additionalMessage = "")
      => Task.Run(() => AssertAwaitAtMostTask(timeoutMs, action, additionalMessage)).Wait();

  private static async Task AssertAwaitAtMostTask(
    long timeoutMs, Action action, string additionalMessage)
  {
    Exception trackedException = new("Empty exception");
    var ranAction = Task.Run(() =>
    {
      while (true)
      {
        try
        {
          action();
          break;
        }
        catch (Exception ex)
        {
          trackedException = ex;
        }
      }
    });

    try
    {
      await ranAction.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
    }
    catch (TimeoutException)
    {
      throw new AssertionException(
        $"Assertion was not passed in time: {timeoutMs}ms. {additionalMessage}", trackedException);
    }
  }
}