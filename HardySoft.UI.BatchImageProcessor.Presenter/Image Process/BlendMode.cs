using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	internal class BlendMode {
		public static readonly Func<int, int, int> Normal = (B, L) => ((B));
		public static readonly Func<int, int, int> Lighten = (B, L) => (((L > B) ? L : B));
		public static readonly Func<int, int, int> Darken = (B, L) => (((L > B) ? B : L));
		public static readonly Func<int, int, int> Multiply = (B, L) => (((B * L) / 255));
		public static readonly Func<int, int, int> Average = (B, L) => (((B + L) / 2));
		public static readonly Func<int, int, int> Add = (B, L) => ((Math.Min(255, (B + L))));
		public static readonly Func<int, int, int> Subtract = (B, L) => (((B + L < 255) ? 0 : (B + L - 255)));
		public static readonly Func<int, int, int> Difference = (B, L) => ((Math.Abs(B - L)));
		public static readonly Func<int, int, int> Negation = (B, L) => ((255 - Math.Abs(255 - B - L)));
		public static readonly Func<int, int, int> Screen = (B, L) => ((255 - (((255 - B) * (255 - L)) >> 8)));
		public static readonly Func<int, int, int> Exclusion = (B, L) => ((B + L - 2 * B * L / 255));
		public static readonly Func<int, int, int> Overlay = (B, L) => (((L < 128) ? (2 * B * L / 255) : (255 - 2 * (255 - B) * (255 - L) / 255)));
		public static readonly Func<int, int, int> SoftLight = (B, L) => (int)(((L < 128) ? (2 * ((B >> 1) + 64)) * ((float)L / 255) : (255 - (2 * (255 - ((B >> 1) + 64)) * (float)(255 - L) / 255))));
		public static readonly Func<int, int, int> HardLight = (B, L) => (Overlay(L, B));
		public static readonly Func<int, int, int> ColorDodge = (B, L) => (((L == 255) ? L : Math.Min(255, ((B << 8) / (255 - L)))));
		public static readonly Func<int, int, int> ColorBurn = (B, L) => (((L == 0) ? L : Math.Max(0, (255 - ((255 - B) << 8) / L))));
		public static readonly Func<int, int, int> LinearDodge = (B, L) => (Add(B, L));
		public static readonly Func<int, int, int> LinearBurn = (B, L) => (Subtract(B, L));
		public static readonly Func<int, int, int> LinearLight = (B, L) => ((L < 128) ? LinearBurn(B, (2 * L)) : LinearDodge(B, (2 * (L - 128))));
		public static readonly Func<int, int, int> VividLight = (B, L) => ((L < 128) ? ColorBurn(B, (2 * L)) : ColorDodge(B, (2 * (L - 128))));
		public static readonly Func<int, int, int> PinLight = (B, L) => ((L < 128) ? Darken(B, (2 * L)) : Lighten(B, (2 * (L - 128))));
		public static readonly Func<int, int, int> HardMix = (B, L) => (((VividLight(B, L) < 128) ? 0 : 255));
		public static readonly Func<int, int, int> Reflect = (B, L) => (((L == 255) ? L : Math.Min(255, (B * B / (255 - L)))));
		public static readonly Func<int, int, int> Glow = (B, L) => (Reflect(L, B));
		public static readonly Func<int, int, int> Phoenix = (B, L) => ((Math.Min(B, L) - Math.Max(B, L) + 255));
	}
}
