using System;

namespace Rick
{
    /// <summary>
    /// Models a discrete resistor.
    /// </summary>
    /// <raises>
    /// <see cref="ResistorException"/> raised when the <see cref="Resistance"/> property cannot be represented by 
    /// an instance of <see cref="System.Int64"/> or when one supplies no multiplier.
    /// </raises>
    /// <remarks>
    /// For simply calculating the resistance, this class is overkill. However, I envision that this application 
    /// eventually supports a QA technician being able to determine the nominal resistance and tolerance of a discrete
    /// resistor. If the QA tech performs tests on resistors, capacitors and other discrete components, having a
    /// <see cref="Resistor"/> class may make sense.
    /// </remarks> 
    public class Resistor
    {
        private readonly BandColor _bandAColor;
        private readonly BandColor _bandBColor;
        private readonly BandColor _bandCColor;
        // ReSharper disable once NotAccessedField.Local
        private BandColor _bandDColor;

        public Resistor(BandColor bandAColor, BandColor bandBColor, BandColor bandCColor, BandColor bandDColor)
        {
            _bandAColor = bandAColor;
            _bandBColor = bandBColor;
            _bandCColor = bandCColor;
            _bandDColor = bandDColor;
        }

        public long Resistance()
        {
            // Handle a zero-ohm resistor specially.
            if (IsZeroOhmResistor)
            {
                return 0;
            }

            var significantFigures = CalculateSignificantFigures();

            var value = ApplyMultiplier(significantFigures);

            return value;
        }

        private long ApplyMultiplier(int significantFigures)
        {
            ValidateMultiplierBand();
            var value = significantFigures*Multiplier;
            return value;
        }

        private int CalculateSignificantFigures()
        {
            ValidateSignificantFigureBands();
            var significantFigures = 10*FirstSignificantDigit + SecondSignificantDigit;
            return significantFigures;
        }

        private void ValidateMultiplierBand()
        {
            if (HasNoMultiplierBand)
            {
                throw new ResistorException("No multiplier band found.");
            }

            if (HasUntranslatableMultiplerBand)
            {
                throw new ResistorException(string.Format("Unhandled {0} multiplier band.",
                    _bandCColor.ToString().ToLowerInvariant()));
            }
        }

        private bool HasUntranslatableMultiplerBand
        {
            get { return _bandCColor == BandColor.Gold || _bandCColor == BandColor.Silver; }
        }

        private bool HasNoMultiplierBand
        {
            get { return _bandCColor == BandColor.None; }
        }

        private void ValidateSignificantFigureBands()
        {
            MustHaveSignificantFigureBandLabeled(_bandAColor, "A");
            MustHaveSignificantFigureBandLabeled(_bandBColor, "B");

            MustHaveTranslatableSignificantFigureBand(_bandAColor, "A");
            MustHaveTranslatableSignificantFigureBand(_bandBColor, "B");
        }

        private static void MustHaveTranslatableSignificantFigureBand(BandColor bandColor, string invalidBand)
        {
            if (bandColor == BandColor.Gold || bandColor == BandColor.Silver)
            {
                throw new ResistorException(
                    string.Format("Cannot convert a {0} {1} band to a significant figure.",
                        bandColor.ToString().ToLowerInvariant(), invalidBand));
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void MustHaveSignificantFigureBandLabeled(BandColor bandColor, string invalidBand)
        {
            if (bandColor == BandColor.None)
            {
                throw new ResistorException(string.Format("Significant figure band {0} not present.", invalidBand));
            }
        }

        private long Multiplier
        {
            // Using the Math library (which uses double precision arithmetic) may not be the most efficient method
            // to calculate integral powers of 10, but it suffices for this problem.
            get
            {
                return (long) Math.Pow(10, (int) _bandCColor);
            }
        }

        private int SecondSignificantDigit
        {
            get { return (int) _bandBColor; }
        }

        private int FirstSignificantDigit
        {
            get { return (int) _bandAColor; }
        }

        private bool IsZeroOhmResistor
        {
            get { return _bandAColor == BandColor.Black && _bandBColor == BandColor.None && _bandCColor == BandColor.None; }
        }
    }
}