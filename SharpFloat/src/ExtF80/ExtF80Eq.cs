using System;
using SharpFloat.Globals;

namespace SharpFloat.ExtF80 {

    public partial struct ExtF80 : IEquatable<ExtF80> {

        public static bool operator ==(ExtF80 l, ExtF80 r) {
            if (IsNaNExtF80UI(l.signExp, l.signif) || IsNaNExtF80UI(r.signExp, r.signif)) {

                if (IsSigNaNExtF80UI(l.signExp, l.signif) || IsSigNaNExtF80UI(r.signExp, r.signif)) {
                    Settings.Raise(ExceptionFlags.Invalid);
                }

                return false;
            }

            return //
                (l.signif == r.signif)
                && ((l.signExp == r.signExp) || (l.signif == 0 && 0 != ((l.signExp | r.signExp) & 0x7FFF)));
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ExtF80 e)
                return e == this;
            return false;
        }

        public bool Equals(ExtF80 other)
            => this == other;

        public override int GetHashCode()
            => (signExp.GetHashCode() * 397) ^ signif.GetHashCode();
    }

}
