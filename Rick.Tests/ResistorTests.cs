﻿using NUnit.Framework;

namespace Rick.Tests
{
    /// <summary>
    /// Defines the unit tests for the <see cref="Resistor"/> class.
    /// </summary>
    /// <remarks>
    /// Here are some additional test ideas:
    /// * The Wikipedia article (https://en.wikipedia.org/wiki/Electronic_color_code) described a five-band resistor. 
    ///   Because this is not needed to support the interface `IOhmValueCalculator`, I chose not to implement it.
    /// * Change `Resistance` to return double. (This allows correct handling of `Gold` and `Silver` bands for the
    ///   multiplier.
    /// </remarks>
    [TestFixture]
    public class ResistorTests
    {
        /// <summary>
        /// Calculate the resistance of a resistor with a single black band.
        /// </summary>
        /// <remarks>
        /// I noticed this example in the Wikipedia article (https://en.wikipedia.org/wiki/Electronic_color_code).
        /// Because it seemed very unusual, I wanted to ensure I handled it correctly.
        /// </remarks>
        [TestCase]
        public void Resistance_SingleBlackBand_ReturnsZero()
        {
            var sut = new Resistor(BandColor.Black, BandColor.None, BandColor.None, BandColor.None);

            Assert.That(sut.Resistance(), Is.Zero);
        }

        [TestCase(BandColor.Yellow, BandColor.Violet, BandColor.Red, BandColor.Gold, 4700)]
        [TestCase(BandColor.Red, BandColor.Red, BandColor.Orange, BandColor.Gold, 22000)]
        [TestCase(BandColor.Yellow, BandColor.Violet, BandColor.Brown, BandColor.Gold, 470)]
        [TestCase(BandColor.Blue, BandColor.Gray, BandColor.Black, BandColor.Gold, 68)]
        [TestCase(BandColor.Brown, BandColor.Black, BandColor.Yellow, BandColor.Gold, 100000)]
        [TestCase(BandColor.Orange, BandColor.Green, BandColor.Green, BandColor.Gold, 3500000)]
        [TestCase(BandColor.Blue, BandColor.Violet, BandColor.Blue, BandColor.Gold, 67000000)]
        [TestCase(BandColor.Gray, BandColor.White, BandColor.Violet, BandColor.Gold, 890000000)]
        [TestCase(BandColor.Black, BandColor.Gray, BandColor.Gray, BandColor.Gold, 800000000)]
        public void Resistance_SpecifiedBands_ReturnsExpectedIntegralValue(BandColor bandAColor, BandColor bandBColor,
            BandColor bandCColor, BandColor bandDColor, int expectedValue)
        {
            var sut = new Resistor(bandAColor, bandBColor, bandCColor, bandDColor);

            Assert.That(sut.Resistance(), Is.EqualTo(expectedValue));
        }

        [TestCase(BandColor.Red, BandColor.White, BandColor.White, BandColor.None, 29000000000L)]
        public void Resistance_SpecifiedBands_ReturnsExpectedLongValue(BandColor bandAColor, BandColor bandBColor,
            BandColor bandCColor, BandColor bandDColor, long expectedValue)
        {
            var sut = new Resistor(bandAColor, bandBColor, bandCColor, bandDColor);

            Assert.That(sut.Resistance(), Is.EqualTo(expectedValue));
        }

        [TestCase(BandColor.None, BandColor.Gray, BandColor.None, BandColor.None, "A")]
        [TestCase(BandColor.Brown, BandColor.None, BandColor.None, BandColor.None, "B")]
        public void Resistance_NoneSignificantFigureBand_ThrowsResistorExceptionWithBandMessage(BandColor bandAColor,
            BandColor bandBColor, BandColor bandCColor, BandColor bandDColor, string invalidBand)
        {
            var sut = new Resistor(bandAColor, bandBColor, bandCColor, bandDColor);

            Assert.That(() => sut.Resistance(),
                Throws.InstanceOf<ResistorException>()
                      .With.Message.EqualTo(string.Format("Significant figure band {0} not present.", invalidBand)));
        }

        [TestCase(BandColor.Gold, BandColor.Gray, BandColor.None, BandColor.None, "A")]
        [TestCase(BandColor.Silver, BandColor.Black, BandColor.None, BandColor.None, "A")]
        public void Resistance_InvalidBandASignificantFigure_ThrowsResistorExceptionWithBandAMessage(BandColor bandAColor,
            BandColor bandBColor, BandColor bandCColor, BandColor bandDColor, string invalidBand)
        {
            var sut = new Resistor(bandAColor, bandBColor, bandCColor, bandDColor);

            Assert.That(() => sut.Resistance(),
                Throws.InstanceOf<ResistorException>()
                      .With.Message.EqualTo(string.Format("Cannot convert a {0} {1} band to a significant figure.",
                          bandAColor.ToString().ToLowerInvariant(), invalidBand)));
        }

        [TestCase(BandColor.Brown, BandColor.Gold, BandColor.None, BandColor.None, "B")]
        [TestCase(BandColor.Green, BandColor.Silver, BandColor.None, BandColor.None, "B")]
        public void Resistance_InvalidBandBSignificantFigure_ThrowsResistorExceptionWithBandAMessage(BandColor bandAColor,
            BandColor bandBColor, BandColor bandCColor, BandColor bandDColor, string invalidBand)
        {
            var sut = new Resistor(bandAColor, bandBColor, bandCColor, bandDColor);

            Assert.That(() => sut.Resistance(),
                Throws.InstanceOf<ResistorException>()
                      .With.Message.EqualTo(string.Format("Cannot convert a {0} {1} band to a significant figure.",
                          bandBColor.ToString().ToLowerInvariant(), invalidBand)));
        }

        [TestCase]
        [Ignore("Not yet implemented.")]
        public void Resistance_NonIntegral_ThrowsResistorExceptionWithMessage()
        {
            var sut = new Resistor(BandColor.Gold, BandColor.Gold, BandColor.Gold, BandColor.None);

            Assert.That(() => sut.Resistance(),
                Throws.InstanceOf<ResistorException>().With.Message.EqualTo("Non-integral resistance '5.5'."));
        }

    }
}
