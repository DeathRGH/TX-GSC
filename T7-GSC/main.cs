using libframe4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using T7_GSC.Properties;

namespace T7_GSC
{
	public partial class main : Form
	{
		FRAME4 ps4;

		public static int processID = -1;
		ulong stub = 0;
		ulong stringbuf = 0;

		int gameVersion = -1;

		Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

		private const int CS_DROPSHADOW = 0x00020000;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= CS_DROPSHADOW;
				return cp;
			}
		}

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x02;

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rc = new Rectangle(this.ClientSize.Width - 16, this.ClientSize.Height - 16, 16, 16);
			ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x84)
			{
				Point pos = new Point(m.LParam.ToInt32());
				pos = this.PointToClient(pos);

				if (pos.X >= this.ClientSize.Width - 16 && pos.Y >= this.ClientSize.Height - 16)
				{
					m.Result = (IntPtr)17;
					return;
				}
			}
			base.WndProc(ref m);
		}

		void UpgradeUserSettings()
		{
			if (Settings.Default.UPDATE_REQUIRED)
			{
				Settings.Default.Upgrade();
				Settings.Default.UPDATE_REQUIRED = false;
				Settings.Default.Save();
			}
		}

		bool IsIPAddress(string check)
		{
			if (System.Net.IPAddress.TryParse(check, out System.Net.IPAddress ip))
				return true;
			else
				return false;
		}

		void ChangeIPAdress(string newIp)
		{
			if (IsIPAddress(newIp))
			{
				Settings.Default.IP = newIp;
				Settings.Default.Save();

				ps4 = new FRAME4(newIp);
			}
		}

		void Log(string text, bool clear = false)
		{
			Invoke((MethodInvoker)delegate
			{
				if (clear)
					label_log.Text = "";

				label_log.Text += $"{text}\n";
			});
		}

        void LogToFile(string text)
        {
            File.AppendAllText($@"{Application.StartupPath}\log.txt", $"{text}\n");
        }

        ulong DB_FindXAssetHeader(BO3.XAssetType type, string name, int allowCreateDefault)
		{
			ps4.WriteString(processID, stringbuf, name);

			switch (gameVersion)
			{
				case 100:
					return ps4.Call(processID, stub, BO3._01_00.Addresses.DB_FindXAssetHeader, (int)type, stringbuf, allowCreateDefault, 0xFFFFFFFF);
				case 133:
					return ps4.Call(processID, stub, BO3._01_33.Addresses.DB_FindXAssetHeader, (int)type, stringbuf, allowCreateDefault, 0xFFFFFFFF);
			}

			return 0;
		}

		public main()
		{
			InitializeComponent();
			UpgradeUserSettings();
			SetStyle(ControlStyles.ResizeRedraw, true);

			title.Text += $"   [{version}]";
			box_ip.Text = Settings.Default.IP;
		}

		private void topBar_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void title_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void minimize_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}

		private void exit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void box_ip_TextChanged(object sender, EventArgs e)
		{
			ChangeIPAdress(box_ip.Text);
		}

		private void btn_connect_Click(object sender, EventArgs e)
		{
			Task.Factory.StartNew(() =>
			{
				Log("Connecting...", true);

				if (string.IsNullOrEmpty(Settings.Default.IP))
				{
					MessageBox.Show("No IP found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				ps4 = new FRAME4(Settings.Default.IP);

				try
				{
					ps4.Connect();
				}
				catch
				{
					MessageBox.Show($"Unable to connect to PS4 at {Settings.Default.IP}!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("Connected.");
				Log("Looking for game...");

				try
				{
					ProcessList pl = ps4.GetProcessList();
					Process p = pl.FindProcess("eboot.bin");
					processID = p.pid;
				}
				catch
				{
					MessageBox.Show($"Unable to find the game process, make sure it is running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("Installing RPC...");

				try
				{
					Util.InstallRemoteProcedureCall(ps4, processID, ref stub, ref stringbuf);
				}
				catch
				{
					MessageBox.Show($"Unable to install the RPC, no function calls are available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (stub == 0 || stringbuf == 0)
				{
					MessageBox.Show($"Failed to install the RPC, no function calls are available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("RPC ready.");
				Log("Detecting game version...");
				Log("(1/2) Version 1.00 ?");

				try
				{
					string tmp = ps4.ReadString(processID, 0x0000000001757A52, 100);
					if (tmp == "multiplayer")
					{
						Log("Version 1.00 detected!");
						gameVersion = 100;
						return;
					}
					else
						Log("-> No, moving on...");
				}
				catch
				{
					MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("(2/2) Version 1.33 ?");

				try
				{
					string tmp = ps4.ReadString(processID, 0x0000000001712302, 100);
					if (tmp == "multiplayer")
					{
						Log("Version 1.33 detected!");
						gameVersion = 133;
						return;
					}
					else
						Log("-> No, game version not supported!");
				}
				catch
				{
					MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			});
		}

		private void btn_fetchLoadedFiles_Click(object sender, EventArgs e)
		{
			try
			{
				listBox_gscFiles.Items.Clear();

				Task.Factory.StartNew(() =>
				{
					Invoke((MethodInvoker)delegate
					{
						Log("Listing files...", true);
					});

					List<ulong> pointerList = new List<ulong>();
                    List<ulong> filePtrList = new List<ulong>();
                    byte[] pointerBuf = new byte[0];

					switch (gameVersion)
					{
						case 100:
							pointerBuf = ps4.ReadMemory(processID, BO3._01_00.Addresses.s_ScriptParseTreePool, 0x10000);
							break;
						case 133:
							pointerBuf = ps4.ReadMemory(processID, BO3._01_33.Addresses.s_ScriptParseTreePool, 0x10000);
							break;
					}

					for (int i = 0; i < 0x10000 / 0x18; i++)
					{
						ulong ptr = BitConverter.ToUInt64(pointerBuf, i * 0x18);
                        ulong filePtr = 0;

                        switch (gameVersion)
                        {
                            case 100:
                                filePtr = BO3._01_00.Addresses.s_ScriptParseTreePool + (ulong)(i * 0x18);
                                break;
                            case 133:
                                filePtr = BO3._01_33.Addresses.s_ScriptParseTreePool + (ulong)(i * 0x18);
                                break;
                        }

                        if (ptr > 0x100000000)
                        {
                            pointerList.Add(ptr);
                            filePtrList.Add(filePtr);
                        }
                        else
                        {
                            break;
                        }
					}

					int numFiles = pointerList.Count;

					for (int i = 0; i < numFiles; i++)
					{
						ulong pointer = pointerList[i];
                        ulong filePointer = filePtrList[i];
                        string assetName = ps4.ReadString(processID, pointer, 100);

						Invoke((MethodInvoker)delegate
						{
							listBox_gscFiles.Items.Add($"0x{filePointer:X16} {assetName}");
                            LogToFile($"0x{filePointer:X16} {assetName}");
                            progressBar_listFiles.Value = (int)(1000.0f / numFiles * i);
							Log($"Listing files... ({i + 1}/{numFiles})", true);

							int visibleItems = listBox_gscFiles.ClientSize.Height / listBox_gscFiles.ItemHeight;
							listBox_gscFiles.TopIndex = Math.Max(listBox_gscFiles.Items.Count - visibleItems + 1, 0);
						});
					}

					Invoke((MethodInvoker)delegate
					{
						progressBar_listFiles.Value = 1000;
					});
				});
			}
			catch (Exception ex)
			{
				Util.DefaultError(ex);
			}
		}

		private void btn_dumpFile_Click(object sender, EventArgs e)
		{
			try
			{
				string gscFileName = listBox_gscFiles.Text;

				Log($"Dumping \"{gscFileName}\"", true);

				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = gscFileName.Split('/')[gscFileName.Split('/').Length - 1];

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					ulong ptr = DB_FindXAssetHeader(BO3.XAssetType.ASSET_TYPE_SCRIPTPARSETREE, gscFileName, 0);
					int size = ps4.ReadInt32(processID, ptr + 0x08);
					ulong filePtr = ps4.ReadUInt64(processID, ptr + 0x10);

					byte[] file = ps4.ReadMemory(processID, filePtr, size);

					File.WriteAllBytes(sfd.FileName, file);
				}
			}
			catch (Exception ex)
			{
				Util.DefaultError(ex);
			}
		}

		private void btn_dumpAllFiles_Click(object sender, EventArgs e)
		{
			try
			{
				Log("Dumping all loaded files...", true);

				FolderBrowserDialog fbd = new FolderBrowserDialog();
				string rootFolder = "";

				if (fbd.ShowDialog() == DialogResult.OK)
					rootFolder = fbd.SelectedPath;
				else
				{
					MessageBox.Show("Unable to locate dump folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("Generating list of files...");

				Task.Factory.StartNew(() =>
				{
					List<ulong> pointerList = new List<ulong>();
					List<int> sizeList = new List<int>();
					List<ulong> filePointerList = new List<ulong>();
					byte[] pointerBuf = new byte[0];

					switch (gameVersion)
					{
						case 100:
							pointerBuf = ps4.ReadMemory(processID, BO3._01_00.Addresses.s_ScriptParseTreePool, 0x10000);
							break;
						case 133:
							pointerBuf = ps4.ReadMemory(processID, BO3._01_33.Addresses.s_ScriptParseTreePool, 0x10000);
							break;
					}

					for (int i = 0; i < 0x10000 / 0x18; i++)
					{
						ulong ptr = BitConverter.ToUInt64(pointerBuf, i * 0x18);
						int size = BitConverter.ToInt32(pointerBuf, i * 0x18 + 0x08);
						ulong filePtr = BitConverter.ToUInt64(pointerBuf, i * 0x18 + 0x10);

						if (ptr > 0x100000000)
						{
							pointerList.Add(ptr);
							sizeList.Add(size);
							filePointerList.Add(filePtr);
						}
						else
							break;
					}

					int numFiles = pointerList.Count;

					for (int i = 0; i < numFiles; i++)
					{
						ulong ptr = pointerList[i];
						string assetName = ps4.ReadString(processID, ptr, 100);
						int size = sizeList[i];
						ulong filePtr = filePointerList[i];

						if (!assetName.Contains("."))
							continue;

						Invoke((MethodInvoker)delegate
						{
							progressBar_listFiles.Value = (int)(1000.0f / numFiles * i);
							Log($"Dumping file {i + 1}/{numFiles}", true);
							Log($"{assetName}");
						});

						string[] folders = assetName.Split('/');
						string currentDir = $"{rootFolder}";
						for (int j = 0; j < folders.Length - 1; j++)
							currentDir += $@"\{folders[j]}";

						if (!Directory.Exists(currentDir))
							Directory.CreateDirectory(currentDir);

						byte[] fileBytes = ps4.ReadMemory(processID, filePtr, size);

						File.WriteAllBytes($@"{currentDir}\{folders[folders.Length - 1]}", fileBytes);
					}

					Invoke((MethodInvoker)delegate
					{
						progressBar_listFiles.Value = 1000;
					});
				});
			}
			catch (Exception ex)
			{
				Util.DefaultError(ex);
			}
		}

		private void btn_makeDataBase_Click(object sender, EventArgs e)
		{
			Log($"Creating Database", true);

			try
			{
				OpenFileDialog ofd = new OpenFileDialog();

				string[] fileLines = null;

				if (ofd.ShowDialog() == DialogResult.OK)
					fileLines = File.ReadAllLines(ofd.FileName);

				byte[] opBytes = new byte[0x4000];
				for (int i = 0; i < opBytes.Length; i++)
					opBytes[i] = 0xFF;

				if (fileLines != null)
				{
					for (int i = 0; i < fileLines.Length; i++)//foreach (string line in fileLines)
					{
						foreach (BO3.ScriptOpCode op in Enum.GetValues(typeof(BO3.ScriptOpCode)))
						{
							if (fileLines[i].Contains(Enum.GetName(typeof(BO3.ScriptOpCode), op)))
							{
								Console.WriteLine($"0x{(byte)op:X2} - {op}");
								opBytes[i] = (byte)op;
							}
						}
					}
				}

				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = "T7PS4.db";

				if (sfd.ShowDialog() == DialogResult.OK)
					File.WriteAllBytes(sfd.FileName, opBytes);
			}
			catch (Exception ex)
			{
				Util.DefaultError(ex);
			}
		}

		private void button_inject_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				//to do:
				//fix 1.00 injection point
				ulong ptr = 0x000000000547EF00; //scripts/shared/duplicaterender_mgr.gsc

				byte[] fileBuf = File.ReadAllBytes(ofd.FileName);

				byte[] ogChecksum = ps4.ReadMemory(processID, ps4.ReadUInt64(processID, ptr) + 0x08, 4);
				Buffer.BlockCopy(ogChecksum, 0, fileBuf, 8, 4);

				ulong newPtr = ps4.AllocateMemory(processID, fileBuf.Length);
				ps4.WriteMemory(processID, newPtr, fileBuf);
				ps4.WriteUInt64(processID, ptr, newPtr);
			}
		}
	}
}
