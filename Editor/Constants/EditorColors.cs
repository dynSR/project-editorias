using UnityEngine;
using Utilitas;

namespace Editorias {
    public static class EditorButtonColors {
        public static Color BackgroundLightThemeIdle => "#E4E4E4".HexToColor();
        public static Color BackgroundLightThemeFocus => "#BEBEBE".HexToColor();
        public static Color BackgroundLightThemeHover => "#ECECEC".HexToColor();
        public static Color BackgroundLightThemeHoverPressed => "#B0D2FC".HexToColor();
        public static Color BackgroundLightThemePressed => "#96C3FB".HexToColor();

        public static Color BackgroundDarkThemeIdle => "#585858".HexToColor();
        public static Color BackgroundDarkThemeFocus => "#6E6E6E".HexToColor();
        public static Color BackgroundDarkThemeHover => "#676767".HexToColor();
        public static Color BackgroundDarkThemeHoverPressed => "#4F657F".HexToColor();
        public static Color BackgroundDarkThemePressed => "#46607C".HexToColor();

        public static Color BorderLightThemeIdle => "#B2B2B2".HexToColor();
        public static Color BorderLightThemeAccent => "#939393".HexToColor();
        public static Color BorderLightThemeAccentFocus => "#018CFF".HexToColor();
        public static Color BorderLightThemePressed => "#707070".HexToColor();

        public static Color BorderDarkThemeIdle => "#303030".HexToColor();
        public static Color BorderDarkThemeAccent => "#242424".HexToColor();
        public static Color BorderDarkThemeAccentFocus => "#7BAEFA".HexToColor();
        public static Color BorderDarkThemePressed => "#0D0D0D".HexToColor();
    }
}