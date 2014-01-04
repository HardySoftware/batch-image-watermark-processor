using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace HardySoft.CC.ExceptionLog {
	public class ExceptionLogger {
		public static ExceptionContainer GetException(Exception ex) {
			ExceptionContainer container = new ExceptionContainer() {
				ExceptionTime = DateTime.Now,
				ThreadIdentity = Thread.CurrentPrincipal.Identity.Name,
				ApplicationDomain = AppDomain.CurrentDomain.FriendlyName,
				ExceptionType = ex.GetType().ToString()
			};

			container.Source = getErrorSource();

			PropertyInfo[] publicProperties = ex.GetType().GetProperties();
			KeyValuePairItem[] items = new KeyValuePairItem[publicProperties.Length];

			int counter = 0;

			// TODO nest inner exception checking
			foreach (PropertyInfo p in publicProperties) {
				KeyValuePairItem item = new KeyValuePairItem();
				if (p.Name == null) {
					item.Key = string.Empty;
				} else {
					item.Key = p.Name;
				}

				object value = p.GetValue(ex, null);
				if (value == null) {
					item.Value = string.Empty;
				} else {
					item.Value = value.ToString();
				}

				if (string.Compare(p.Name, "StackTrace", true) == 0) {
					container.Stacks = parseStackTrace(item.Value);
				}

				items[counter] = item;

				counter++;
			}

			container.ExceptionDetail = items;

			return container;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		private static string getErrorSource() {
			StringBuilder preamble = new StringBuilder();

			StackTrace stackTrace = new StackTrace();
			StackFrame stackFrame;
			MethodBase stackFrameMethod;

			int frameCount = 0;
			string typeName;
			do {
				frameCount++;
				stackFrame = stackTrace.GetFrame(frameCount);
				stackFrameMethod = stackFrame.GetMethod();
				typeName = stackFrameMethod.ReflectedType.FullName;
			} while (typeName.StartsWith("System") || typeName.StartsWith("HardySoft.CC"));

			preamble.Append(typeName);
			preamble.Append(".");
			preamble.Append(stackFrameMethod.Name);
			preamble.Append("( ");

			// log parameter types and names
			ParameterInfo[] parameters = stackFrameMethod.GetParameters();
			int parameterIndex = 0;
			while (parameterIndex < parameters.Length) {
				preamble.Append(parameters[parameterIndex].ParameterType.Name);
				preamble.Append(" ");
				preamble.Append(parameters[parameterIndex].Name);
				parameterIndex++;
				if (parameterIndex != parameters.Length) {
					preamble.Append(", ");
				}
			}

			preamble.Append(" ): ");

			return preamble.ToString();
		}

		private static CallStack[] parseStackTrace(string stackTrace) {
			string[] stacks = Regex.Split(stackTrace, "\r\n");

			CallStack[] stackCollection = new CallStack[stacks.Length];

			for (int i = 0; i < stacks.Length; i++) {
				CallStack stack = new CallStack();
				stack.Sequence = stacks.Length - i;
				stack.Stack = stacks[i];

				stackCollection[i] = stack;
			}

			return stackCollection;
		}
	}
}
