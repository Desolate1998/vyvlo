import { BrandVariants, Theme, createLightTheme, createDarkTheme } from "@fluentui/react-components";

const applicationTheme: BrandVariants = {
    10: "#020207",
    20: "#10162A",
    30: "#162348",
    40: "#1C2E60",
    50: "#243A76",
    60: "#2E468A",
    70: "#3B529D",
    80: "#4A5FAE",
    90: "#5A6CBC",
    100: "#6B7ACA",
    110: "#7C88D6",
    120: "#8D96E0",
    130: "#9EA5E9",
    140: "#AFB4F1",
    150: "#C1C3F7",
    160: "#D2D3FC"
};

const lightTheme: Theme = {
    ...createLightTheme(applicationTheme),
};

const darkTheme: Theme = {
    ...createDarkTheme(applicationTheme),
};


export {lightTheme, darkTheme};