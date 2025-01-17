using System;
using System.Windows.Forms;

namespace T8_GSC
{
	class Util
	{
		public static void DefaultError(Exception ex)
		{
			MessageBox.Show($"{ex.Message}\n\n{ex.HResult}", $"HResult 0x{ex.HResult.ToString("X")} - {ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static string NullTerminatedStringFromByteArray(byte[] byteArray)
		{
			string str = "";
			int i = 0;

			while (true)
			{
				byte value = byteArray[i];

				if (value == 0)
					break;

				str += Convert.ToChar(value);
				i++;
			}

			return str;
		}

		public static void OpenForm(Form form)
		{
			FormCollection fc = Application.OpenForms;

			foreach (Form f in fc)
				if (f.Text == form.Text)
					return;

			try
			{
				form.Show();
			}
			catch (Exception ex)
			{
				DefaultError(ex);
			}
		}

		public static void InstallRemoteProcedureCall(libframe4.FRAME4 ps4, int pid, ref ulong stub, ref ulong stringbuf)
		{
			try
			{
				stub = ps4.InstallRPC(pid);
				stringbuf = ps4.AllocateMemory(pid, 0x1000);
			}
			catch (Exception ex)
			{
				DefaultError(ex);
			}
		}
	}
}
