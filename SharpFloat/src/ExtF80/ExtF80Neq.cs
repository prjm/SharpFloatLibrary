using SharpFloat.Globals;

namespace SharpFloat.ExtF80 {

    public partial struct ExtF80 {

        public static bool operator !=(ExtF80 l, ExtF80 r) {
            if (IsNaNExtF80UI(l.signExp, l.signif) || IsNaNExtF80UI(r.signExp, r.signif)) {

                if (IsSigNaNExtF80UI(l.signExp, l.signif) || IsSigNaNExtF80UI(r.signExp, r.signif)) {
                    Settings.Raise(ExceptionFlags.Invalid);
                }

                return true;
            }

            return //
                (l.signif != r.signif)
                || ((l.signExp != r.signExp) && (l.signif != 0 || 0 != ((l.signExp | r.signExp) & 0x7FFF)));
        }


    }
}
