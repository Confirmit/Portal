using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace ConfirmIt.PortalLib.IPRangeChecker
{
	public class IPRangeChecker
	{
		public static bool IsIPValid(IPAddress ip, string ipRange)
		{
			var result = false;

			if (ipRange.Contains("*"))
			{
				result = IsIPValidMask(ip, ipRange);
			}
			else
			{
				if (ipRange.Contains("*"))
					throw new NotSupportedException("IP range cannot contains mask specification.");

				var ips = ipRange.Split('-');

				var lower = ips[0].Trim();
				var upper = ips.Length > 1 ? ips[1].Trim() : lower;

				result = IsIPInRange(ip, IPAddress.Parse(lower), IPAddress.Parse(upper));
			}

			return result;
		}

		public static bool IsIPValidMask(IPAddress ip, string ipMask)
		{
			if (ipMask.Contains("-"))
				throw  new NotSupportedException("IP mask cannot contains range specification.");

			var ipParts = ipMask.Split('.');

			var pattern = new StringBuilder();

			foreach (var part in ipParts)
			{
				if (pattern.Length != 0)
					pattern.Append(@"\.");

				pattern.AppendFormat(@"\s*{0}\s*", part.Replace("*", @"\d+"));
			}

			pattern.Insert(0, "^");
			pattern.Append("$");

			var regex = new Regex(pattern.ToString());

			return regex.IsMatch(ip.ToString());
		}

		public static bool IsIPInRange(IPAddress address, IPAddress lower, IPAddress upper)
		{
			if (address.AddressFamily != lower.AddressFamily)
				return false;

			var addressBytes = address.GetAddressBytes();

			bool lowerBoundary = true, upperBoundary = true;

			var lowerBytes = lower.GetAddressBytes();
			var upperBytes = upper.GetAddressBytes();

			for (var i = 0; i < lowerBytes.Length && (lowerBoundary || upperBoundary); i++)
			{
				if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
					(upperBoundary && addressBytes[i] > upperBytes[i]))
				{
					return false;
				}

				lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
				upperBoundary &= (addressBytes[i] == upperBytes[i]);
			}

			return true;
		}
	}
}