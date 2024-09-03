using StarterLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StarterTest
{
    public class MultiTaskTest
    {
        #region IsPoweroftwoTest

        [Fact]
        public void IsNumberPowerOfTwo_ReturnsTrue_ForPowerOfTwo()
        {
            // Arrange & Act
            bool result1 = MultiTask.IsNumberPowerOfTwo(1);    
            bool result2 = MultiTask.IsNumberPowerOfTwo(2);    
            bool result3 = MultiTask.IsNumberPowerOfTwo(4);    
            bool result4 = MultiTask.IsNumberPowerOfTwo(8);    
            bool result5 = MultiTask.IsNumberPowerOfTwo(16);   
            bool result6 = MultiTask.IsNumberPowerOfTwo(1024); 

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);
            Assert.True(result4);
            Assert.True(result5);
            Assert.True(result6);
        }

        [Fact]
        public void IsNumberPowerOfTwo_ReturnsFalse_ForNonPowerOfTwo()
        {
            // Arrange & Act
            bool result1 = MultiTask.IsNumberPowerOfTwo(0);   
            bool result2 = MultiTask.IsNumberPowerOfTwo(-2);  
            bool result3 = MultiTask.IsNumberPowerOfTwo(3);   
            bool result4 = MultiTask.IsNumberPowerOfTwo(5);   
            bool result5 = MultiTask.IsNumberPowerOfTwo(18);  

            // Assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.False(result3);
            Assert.False(result4);
            Assert.False(result5);
        }

        [Theory]
        [InlineData(1, true)]   
        [InlineData(2, true)]   
        [InlineData(3, false)]  
        [InlineData(4, true)]  
        [InlineData(5, false)]  
        [InlineData(16, true)]  
        [InlineData(31, false)] 
        [InlineData(32, true)]  
        [InlineData(-2, false)] 
        [InlineData(0, false)]  
        public void IsNumberPowerOfTwo_WithInlineData(int num, bool expected)
        {
            // Act
            bool result = MultiTask.IsNumberPowerOfTwo(num);

            // Assert
            Assert.Equal(expected, result);
        }
        #endregion

        #region ReverseStringTest
        [Fact]
        public void ReverseString_ReturnsReversedString_ForNonEmptyString()
        {
            // Arrange
            string input = "Hello";
            string expected = "olleH";

            // Act
            string result = MultiTask.ReverseString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReverseString_ReturnsEmptyString_ForEmptyString()
        {
            // Arrange
            string input = "";
            string expected = "";

            // Act
            string result = MultiTask.ReverseString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReverseString_ReturnsSameString_ForSingleCharacter()
        {
            // Arrange
            string input = "A";
            string expected = "A";

            // Act
            string result = MultiTask.ReverseString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReverseString_ReturnsReversedString_ForPalindrome()
        {
            // Arrange
            string input = "madam";
            string expected = "madam";

            // Act
            string result = MultiTask.ReverseString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReverseString_ReturnsReversedString_WithSpecialCharacters()
        {
            // Arrange
            string input = "Hello, World!";
            string expected = "!dlroW ,olleH";

            // Act
            string result = MultiTask.ReverseString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region ReplicateStringTest
        [Fact]
        public void ReplicateString_ReturnsEmptyString_WhenTimesIsZero()
        {
            // Arrange
            string input = "Test";
            int times = 0;

            // Act
            string result = MultiTask.ReplicateString(input, times);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ReplicateString_ReturnsEmptyString_WhenTimesIsNegative()
        {
            // Arrange
            string input = "Test";
            int times = -5;

            // Act
            string result = MultiTask.ReplicateString(input, times);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ReplicateString_ReturnsSingleInstance_WhenTimesIsOne()
        {
            // Arrange
            string input = "Test";
            int times = 1;

            // Act
            string result = MultiTask.ReplicateString(input, times);

            // Assert
            Assert.Equal("Test", result);
        }

        [Fact]
        public void ReplicateString_ReturnsReplicatedString_ForMultipleTimes()
        {
            // Arrange
            string input = "Test";
            int times = 3;

            // Act
            string result = MultiTask.ReplicateString(input, times);

            // Assert
            Assert.Equal("TestTestTest", result);
        }

        [Fact]
        public void ReplicateString_ReturnsEmptyString_ForEmptyInput()
        {
            // Arrange
            string input = "";
            int times = 3;

            // Act
            string result = MultiTask.ReplicateString(input, times);

            // Assert
            Assert.Equal("", result);
        }
        #endregion

        #region oddNumberTest
        [Fact]
        public void PrintOddNumbers_PrintsCorrectOddNumbers()
        {
            // Arrange
            var expectedOutput = string.Join(Environment.NewLine, GetExpectedOddNumbers()) + Environment.NewLine;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                MultiTask.PrintOddNumbers();

                // Assert
                var result = sw.ToString();
                Assert.Equal(expectedOutput, result);
            }
        }

        private int[] GetExpectedOddNumbers()
        {
            int[] oddNumbers = new int[50];
            int index = 0;

            for (int i = 1; i < 100; i += 2)
            {
                oddNumbers[index++] = i;
            }

            return oddNumbers;
        }
        #endregion
    }
}
