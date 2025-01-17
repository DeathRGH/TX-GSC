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
using T8_GSC.Properties;

namespace T8_GSC
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

		ulong DB_FindXAssetHeader(BO4.XAssetType type, string name, int allowCreateDefault)
		{
			ps4.WriteString(processID, stringbuf, name);

			switch (gameVersion)
			{
				case 125:
					return ps4.Call(processID, stub, BO4._01_25.Addresses.DB_FindXAssetHeader, (int)type, stringbuf, allowCreateDefault, 1, 0xFFFFFFFF);
			}

			return 0;
		}

		private static ulong Com_Hash(string input)
		{
			ulong hash = 0xCBF29CE484222325;

			foreach (char c in input)
				hash = ((char.ToLower(c) ^ hash) * 0x100000001B3) & 0x7FFFFFFFFFFFFFFF;

			return hash;
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

				if (checkBox_t9.Checked)
					Log("(1/1) Version 1.27 ?");
				else
					Log("(1/2) Version 1.25 ?");

				try
				{
					if (checkBox_t9.Checked)
					{
						string tmp = ps4.ReadString(processID, 0x00000000021787BF, 100);
						if (tmp == "Multiplayer")
						{
							Log("Version 1.27 detected!");
							gameVersion = 127;
							return;
						}
						else
						{
							Log("-> No, game version not supported!");
						}
					}
					else
					{
						string tmp = ps4.ReadString(processID, 0x0000000001DCBF62, 100);
						if (tmp == "Multiplayer")
						{
							Log("Version 1.25 detected!");
							gameVersion = 125;
							return;
						}
						else
						{
							Log("-> No, moving on...");
						}
					}
				}
				catch
				{
					MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Log("(2/2) Version 1.26 ?");

				try
				{
					if (checkBox_t9.Checked)
					{

					}
					else
					{
						string tmp = ps4.ReadString(processID, 0x0000000001DA9D5E, 100);
						if (tmp == "Multiplayer")
						{
							Log("Version 1.26 detected!");
							gameVersion = 126;
							return;
						}
						else
						{
							Log("-> No, game version not supported!");
						}
					}
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

					List<ulong> hashedNameList = new List<ulong>();
					ulong poolPtr = 0;

					switch (gameVersion)
					{
						case 125: //t8
							poolPtr = ps4.ReadUInt64(processID, BO4._01_25.Addresses.s_ScriptParseTreePool);
							break;
						case 126: //t8
							poolPtr = ps4.ReadUInt64(processID, BO4._01_26.Addresses.s_ScriptParseTreePool);
							break;
						case 127: //t9
							poolPtr = ps4.ReadUInt64(processID, 0x5853900);
							break;
					}

					if (poolPtr <= 0)
					{
						MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					byte[] pointerBuf = ps4.ReadMemory(processID, poolPtr, 0x10000);

					for (int i = 0; i < 0x10000 / (checkBox_t9.Checked ? 0x18 : 0x20); i++)
					{
						ulong hashedName = BitConverter.ToUInt64(pointerBuf, i * (checkBox_t9.Checked ? 0x18 : 0x20));

                        File.AppendAllLines($@"{Application.StartupPath}\log.txt", new string[] { $"0x{hashedName:X16} - 0x{(poolPtr + (ulong)(i * (checkBox_t9.Checked ? 0x18 : 0x20))):X16}" });

						if (hashedName > 0xFFFFFFFFFF)
							hashedNameList.Add(hashedName);
						else
							break;
					}

					for (int i = 0; i < hashedNameList.Count; i++)
					{
						string assetName = $"0x{hashedNameList[i]:X16}";

						Invoke((MethodInvoker)delegate
						{
							listBox_gscFiles.Items.Add(assetName);
							progressBar_listFiles.Value = (int)(1000.0f / hashedNameList.Count * i);
							Log($"Listing files... ({i + 1}/{hashedNameList.Count})", true);

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
				if (checkBox_t9.Checked)
				{
					MessageBox.Show("Individual files dumping only supported on T8!");
					return;
				}

				string gscFileName = listBox_gscFiles.Text;

				Log($"Dumping \"{gscFileName}\"", true);

				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = gscFileName.Split('/')[gscFileName.Split('/').Length - 1];

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					ulong ptr = DB_FindXAssetHeader(BO4.XAssetType.ASSET_TYPE_SCRIPTPARSETREE, gscFileName, 0);
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
					List<ulong> hashedNameList = new List<ulong>();
					List<int> sizeList = new List<int>();
					List<ulong> filePointerList = new List<ulong>();

					ulong poolPtr = 0;

					switch (gameVersion)
					{
						case 125: //t8
							poolPtr = ps4.ReadUInt64(processID, BO4._01_25.Addresses.s_ScriptParseTreePool);
							break;
						case 126: //t8
							poolPtr = ps4.ReadUInt64(processID, BO4._01_26.Addresses.s_ScriptParseTreePool);
							break;
						case 127: //t9
							poolPtr = ps4.ReadUInt64(processID, 0x5853900);
							break;
					}

					if (poolPtr <= 0)
					{
						MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					byte[] pointerBuf = ps4.ReadMemory(processID, poolPtr, 0x10000);

					for (int i = 0; i < 0x10000 / (checkBox_t9.Checked ? 0x18 : 0x20); i++)
					{
						ulong hashedName = BitConverter.ToUInt64(pointerBuf, i * (checkBox_t9.Checked ? 0x18 : 0x20));

						if (hashedName > 0xFFFFFFFFFF)
						{
							int size = BitConverter.ToInt32(pointerBuf, i * (checkBox_t9.Checked ? 0x18 : 0x20) + (checkBox_t9.Checked ? 0x10 : 0x18));
							ulong filePtr = BitConverter.ToUInt64(pointerBuf, i * (checkBox_t9.Checked ? 0x18 : 0x20) + (checkBox_t9.Checked ? 0x08 : 0x10));
							sizeList.Add(size);
							filePointerList.Add(filePtr);
							hashedNameList.Add(hashedName);
						}
						else
							break;
					}

					int numFiles = filePointerList.Count;

					for (int i = 0; i < numFiles; i++)
					{
						string assetName = $"{hashedNameList[i]:X16}";
						int size = sizeList[i];
						ulong filePtr = filePointerList[i];

						Invoke((MethodInvoker)delegate
						{
							progressBar_listFiles.Value = (int)(1000.0f / numFiles * i);
							Log($"Dumping file {i + 1}/{numFiles}", true);
							Log($"{StringTable.attemptNameForHash(assetName).Replace(".gsc", "")}.gsc");
						});

						//string[] folders = assetName.Split('/');
						//string currentDir = $"{rootFolder}";
						//for (int j = 0; j < folders.Length - 1; j++)
						//	currentDir += $@"\{folders[j]}";

						//if (!Directory.Exists(currentDir))
						//	Directory.CreateDirectory(currentDir);

						byte[] fileBytes = ps4.ReadMemory(processID, filePtr, size);

						//File.WriteAllBytes($@"{currentDir}\{folders[folders.Length - 1]}", fileBytes);
						File.WriteAllBytes($@"{rootFolder}\{StringTable.attemptNameForHash(assetName).Replace(".gsc", "").Replace("/", "_")}.gsc", fileBytes);
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
			if (checkBox_t9.Checked)
			{
				MessageBox.Show("Database creation only supported on T8!");
				return;
			}

			Log($"Creating Database", true);

			try
			{
				OpenFileDialog ofd = new OpenFileDialog();

				string[] fileLines = null;

				if (ofd.ShowDialog() == DialogResult.OK)
					fileLines = File.ReadAllLines(ofd.FileName);

				byte[] opBytes = new byte[0x1000];
				for (int i = 0; i < opBytes.Length; i++)
					opBytes[i] = 0xFF;

				if (fileLines != null)
				{
					for (int i = 0; i < fileLines.Length; i++)//foreach (string line in fileLines)
					{
						foreach (BO4.ScriptOpCode op in Enum.GetValues(typeof(BO4.ScriptOpCode)))
						{
							if (fileLines[i].Contains(Enum.GetName(typeof(BO4.ScriptOpCode), op)))
							{
								Console.WriteLine($"0x{(byte)op:X2} - {op}");
								opBytes[i] = (byte)op;
							}
						}
					}
				}

				byte[] header = new byte[] { 0x36, 0x00, 0x00, 0x10 };
				byte[] finalBytes = new byte[0x1004];

				Array.Copy(header, 0, finalBytes, 0, header.Length);
				Array.Copy(opBytes, 0, finalBytes, 4, opBytes.Length);

				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = "vm_codes.db2";

				if (sfd.ShowDialog() == DialogResult.OK)
					File.WriteAllBytes(sfd.FileName, finalBytes);
			}
			catch (Exception ex)
			{
				Util.DefaultError(ex);
			}
		}

		private void button_inject_Click(object sender, EventArgs e)
		{
			if (checkBox_t9.Checked)
			{
				MessageBox.Show("GSC injection only supported on T8!");
				return;
			}

			byte[] fileBuf = new byte[0];
			OpenFileDialog ofd = new OpenFileDialog();

			if (ofd.ShowDialog() == DialogResult.OK)
				fileBuf = File.ReadAllBytes(ofd.FileName);

			ulong hash_clientids_shared = 0x124CECFF7280BE52; //DB_FindXAssetHeader(BO4.XAssetType.ASSET_TYPE_SCRIPTPARSETREE, "scripts/core_common/clientids_shared.gsc", 0);
			ulong hash_load = 0x5F85E82374C52031; //scripts/zm_common/load.gsc

			List<ulong> hashedNameList = new List<ulong>();
			List<int> sizeList = new List<int>();
			List<ulong> filePointerList = new List<ulong>();

			ulong poolPtr = 0;

			switch (gameVersion)
			{
				case 125:
					poolPtr = ps4.ReadUInt64(processID, BO4._01_25.Addresses.s_ScriptParseTreePool);
					break;
				case 126:
					poolPtr = ps4.ReadUInt64(processID, BO4._01_26.Addresses.s_ScriptParseTreePool);
					break;
			}

			if (poolPtr <= 0)
			{
				MessageBox.Show($"Unable to read game memory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			byte[] pointerBuf = ps4.ReadMemory(processID, poolPtr, 0x10000);

			for (int i = 0; i < 0x10000 / 0x20; i++)
			{
				ulong hashedName = BitConverter.ToUInt64(pointerBuf, i * 0x20);

				if (hashedName > 0xFFFFFFFFFF)
				{
					int size = BitConverter.ToInt32(pointerBuf, i * 0x20 + 0x18);
					ulong filePtr = poolPtr + ((ulong)i * 0x20) + 0x10;
					sizeList.Add(size);
					filePointerList.Add(filePtr);
					hashedNameList.Add(hashedName);
				}
				else
					break;
			}

			for (int i = 0; i < filePointerList.Count; i++)
			{
				if (hashedNameList[i] == hash_load)
				{
					ulong fileAddress = ps4.ReadUInt64(processID, filePointerList[i]);

					int includeOffset = ps4.ReadInt32(processID, fileAddress + 0x18);
					int includeCount = ps4.ReadInt32(processID, fileAddress + 0x58);

					ps4.WriteUInt64(processID, fileAddress + (ulong)includeOffset + ((ulong)includeCount * 8), hash_clientids_shared);
					ps4.WriteInt32(processID, fileAddress + 0x58, includeCount + 1);

					break;
				}
			}

			for (int i = 0; i < filePointerList.Count; i++)
			{
				if (hashedNameList[i] == hash_clientids_shared)
				{
					ulong fileAddress = ps4.ReadUInt64(processID, filePointerList[i]);

					byte[] ogChecksum = ps4.ReadMemory(processID, fileAddress + 0x08, 4);
					Buffer.BlockCopy(ogChecksum, 0, fileBuf, 8, 4);

					ulong newPtr = ps4.AllocateMemory(processID, fileBuf.Length);
					ps4.WriteMemory(processID, newPtr, fileBuf);
					ps4.WriteUInt64(processID, filePointerList[i], newPtr);

					break;
				}
				
				//byte[] fileBytes = ps4.ReadMemory(processID, filePtr, size);
			}

			//byte[] ogChecksum = ps4.ReadMemory(processID, ps4.ReadUInt64(processID, ptr) + 0x08, 4);
			//Buffer.BlockCopy(ogChecksum, 0, fileBuf, 8, 4);

			//ulong newPtr = ps4.AllocateMemory(processID, fileBuf.Length);
			//ps4.WriteMemory(processID, newPtr, fileBuf);
			//ps4.WriteUInt64(processID, ptr, newPtr);
		}

		private void btn_hash_Click(object sender, EventArgs e)
		{
			box_hashOut.Text = $"0x{Com_Hash(box_hashIn.Text):X16}";
		}
	}

	class StringTable
	{
		private static Dictionary<string, string> knownStringsMap = new Dictionary<string, string>()
		{
			{"7807478A0261BA21", "scripts/core_common/aat_shared.gsc"},
			{"070F74A0D17F019E", "scripts/core_common/ai_puppeteer_shared.gsc"},
			{"3667502A29D67CDD", "scripts/core_common/ai_shared.gsc"},
			{"6B9997619B1320D9", "scripts/core_common/ammo_shared.gsc"},
			{"0E1C794216A471A5", "scripts/core_common/animation_debug_shared.gsc"},
			{"6C004EB571EDF93D", "scripts/core_common/animation_shared.gsc"},
			{"4C2AFE8D5311F9D0", "scripts/core_common/armor.gsc"},
			{"6316B397DD0FA158", "scripts/core_common/array_shared.gsc"},
			{"041F536EADF0B839", "scripts/core_common/audio_shared.gsc"},
			{"32E570F6FC499B57", "scripts/core_common/barricades_shared.gsc"},
			{"557343DF0224D3A8", "scripts/core_common/battlechatter.gsc"},
			{"5314390C9ED06B9B", "scripts/core_common/bb_shared.gsc"},
			{"727471E8652FF423", "scripts/core_common/burnplayer.gsc"},
			{"07C165117833E69F", "scripts/core_common/callbacks_shared.gsc"},
			{"5436E8AAA74A8C8D", "scripts/core_common/challenges_shared.gsc"},
			{"0E2CFE5638ABF8A7", "scripts/core_common/class_shared.gsc"},
			{"487D0473700E6140", "scripts/core_common/clientfield_shared.gsc"},
			{"124CECFF7280BE52", "scripts/core_common/clientids_shared.gsc"},
			{"655414919E2B192F", "scripts/core_common/colors_shared.gsc"},
			{"06CD7236E142843F", "scripts/core_common/compass.gsc"},
			{"232064FA7A8477D3", "scripts/core_common/containers_shared.gsc"},
			{"36526E2CC8AAD43E", "scripts/core_common/contracts_shared.gsc"},
			{"1255E5C30F57FB6E", "scripts/core_common/damage.gsc"},
			{"04710A5C9E49CEA1", "scripts/core_common/damagefeedback_shared.gsc"},
			{"01200CDE2B6EB770", "scripts/core_common/death_circle.gsc"},
			{"0BC6375BE189B89A", "scripts/core_common/debug_shared.gsc"},
			{"0E0EC0141D54BDCC", "scripts/core_common/delete.gsc"},
			{"6F1C72288075F742", "scripts/core_common/demo_shared.gsc"},
			{"3CB2CD0CDABECE79", "scripts/core_common/destructible.gsc"},
			{"4A6A044522BA4E46", "scripts/core_common/dev_shared.gsc"},
			{"531DD096D7362158", "scripts/core_common/dogtags.gsc"},
			{"35D9F847413A1164", "scripts/core_common/doors_shared.gsc"},
			{"43ECE2E42028C409", "scripts/core_common/drown.gsc"},
			{"2D629C8052A1D0AC", "scripts/core_common/entityheadicons_shared.gsc"},
			{"628FB732CFE5766C", "scripts/core_common/events_shared.gsc"},
			{"396E8464382B1054", "scripts/core_common/exploder_shared.gsc"},
			{"090BA9F91705AB61", "scripts/core_common/flag_shared.gsc"},
			{"31C5100E4B72AD61", "scripts/core_common/fx_shared.gsc"},
			{"470C1B0CFE934751", "scripts/core_common/gameobjects_shared.gsc"},
			{"7F2F4BF8F8AC431C", "scripts/core_common/gamestate.gsc"},
			{"4751001D2FB6975D", "scripts/core_common/gametype_shared.gsc"},
			{"22DFAACDBA52F713", "scripts/core_common/gestures.gsc"},
			{"0C5CB032D21C496E", "scripts/core_common/hackable.gsc"},
			{"0509E177613BFBE9", "scripts/core_common/healthoverlay.gsc"},
			{"2C62F02DFB42A6DD", "scripts/core_common/hostmigration_shared.gsc"},
			{"1A0D841D6D472E34", "scripts/core_common/hud_message_shared.gsc"},
			{"4014F8414509F884", "scripts/core_common/hud_shared.gsc"},
			{"67A865EB50D80AD9", "scripts/core_common/hud_util_shared.gsc"},
			{"37F824AFF8872C17", "scripts/core_common/influencers_shared.gsc"},
			{"5CAB158FD07ECA02", "scripts/core_common/killcam_shared.gsc"},
			{"515DBAEED302669F", "scripts/core_common/laststand_shared.gsc"},
			{"0BC09CBE2EA7AA8F", "scripts/core_common/lightning_chain.gsc"},
			{"07B9A35F26EC451B", "scripts/core_common/loadout_shared.gsc"},
			{"00FD2E9FE6934867", "scripts/core_common/load_shared.gsc"},
			{"16D356398D443541", "scripts/core_common/lui_shared.gsc"},
			{"5A54790480D2CA30", "scripts/core_common/match_record.gsc"},
			{"49E73DD2E9D9D345", "scripts/core_common/math_shared.gsc"},
			{"38541E6C71332773", "scripts/core_common/medals_shared.gsc"},
			{"6C08EC9FE1092B5F", "scripts/core_common/microwave_turret_shared.gsc"},
			{"68038994CD1AB0B1", "scripts/core_common/mission_shared.gsc"},
			{"35AA65FF4C21D3A6", "scripts/core_common/music_shared.gsc"},
			{"07C4330D493538BF", "scripts/core_common/oob.gsc"},
			{"76E50B99DB506A18", "scripts/core_common/path.gsc"},
			{"6CC3CC98933BF246", "scripts/core_common/perks.gsc"},
			{"5659D38E0E64DFEE", "scripts/core_common/persistence_shared.gsc"},
			{"55DE4C49F49942A3", "scripts/core_common/placeables.gsc"},
			{"35B54631A63E20E2", "scripts/core_common/popups_shared.gsc"},
			{"5FD3E7B40B64D935", "scripts/core_common/potm_shared.gsc"},
			{"348D545085106679", "scripts/core_common/rank_shared.gsc"},
			{"33FA04A9164D5682", "scripts/core_common/rat_shared.gsc"},
			{"0D370FB7E32D1A89", "scripts/core_common/scene_actor_shared.gsc"},
			{"21A83264140CE843", "scripts/core_common/scene_debug_shared.gsc"},
			{"1A843D46F87C0731", "scripts/core_common/scene_model_shared.gsc"},
			{"00E704EAE9029812", "scripts/core_common/scene_objects_shared.gsc"},
			{"40FB55C49530B5DB", "scripts/core_common/scene_player_shared.gsc"},
			{"09864277E5B10D87", "scripts/core_common/scene_shared.gsc"},
			{"4D5705850129A0A2", "scripts/core_common/scene_vehicle_shared.gsc"},
			{"5B2820E3F284D1D6", "scripts/core_common/scoreevents_shared.gsc"},
			{"01E43095A9E1D964", "scripts/core_common/scriptmodels_shared.gsc"},
			{"76920857AB5B1606", "scripts/core_common/serverfaceanim_shared.gsc"},
			{"34120912910FCFE4", "scripts/core_common/serverfield_shared.gsc"},
			{"699FD0D868285910", "scripts/core_common/simple_hostmigration.gsc"},
			{"50FB8D1D7833F45E", "scripts/core_common/sound_shared.gsc"},
			{"043899C4C8DF0112", "scripts/core_common/spawnbeacon_shared.gsc"},
			{"25BB30AA847CD037", "scripts/core_common/spawner_shared.gsc"},
			{"45D68EFB410A3B94", "scripts/core_common/spawning_shared.gsc"},
			{"5A710679D70EFBFB", "scripts/core_common/spectating.gsc"},
			{"5DFFCC49DF28FD81", "scripts/core_common/statemachine_shared.gsc"},
			{"0D7F44475AFD6320", "scripts/core_common/string_shared.gsc"},
			{"4DF2B1C72D43E7FE", "scripts/core_common/struct.gsc"},
			{"39C5D5881CC195B0", "scripts/core_common/system_shared.gsc"},
			{"7C17B6FA2636756B", "scripts/core_common/table_shared.gsc"},
			{"65119E6B8DCA1002", "scripts/core_common/teleport_shared.gsc"},
			{"2591C23EAE3130FB", "scripts/core_common/throttle_shared.gsc"},
			{"03443FD9221682E7", "scripts/core_common/tracker_shared.gsc"},
			{"6AC1A3FA16D8F191", "scripts/core_common/traps_deployable.gsc"},
			{"67CC4379F6CA40AF", "scripts/core_common/trigger_shared.gsc"},
			{"7615F306C7C95879", "scripts/core_common/turret_shared.gsc"},
			{"462D60E4C4BCEFC4", "scripts/core_common/tweakables_shared.gsc"},
			{"3A7E21B450C1B143", "scripts/core_common/util_shared.gsc"},
			{"1A4F964613635F35", "scripts/core_common/values_shared.gsc"},
			{"311C9E9A2F691298", "scripts/core_common/vehicleriders_shared.gsc"},
			{"4F87655F1E9E4792", "scripts/core_common/vehicle_ai_shared.gsc"},
			{"7A6837D343CDFC0C", "scripts/core_common/vehicle_death_shared.gsc"},
			{"70627F1E9092C1A7", "scripts/core_common/vehicle_drivable_shared.gsc"},
			{"4694CD865AE8D71B", "scripts/core_common/vehicle_shared.gsc"},
			{"09ED9199A567E400", "scripts/core_common/visionset_mgr_shared.gsc"},
			{"1B501FB1A629AEC8", "scripts/core_common/weapons_shared.gsc"},
			{"14189043C8FAA6C0", "scripts/core_common/bots/bot_action.gsc"},
			{"244B0E676A8FDEA2", "scripts/cp_common/art.gsc"},
			{"301BE864DD7B9BE3", "scripts/cp_common/bb.gsc"},
			{"04C9C3EFC1B5241F", "scripts/cp_common/callbacks.gsc"},
			{"3AB26D39B2916E45", "scripts/cp_common/challenges.gsc"},
			{"7A746CDEE4E7DA7C", "scripts/cp_common/cheat.gsc"},
			{"37DD572972F8E118", "scripts/cp_common/collectibles.gsc"},
			{"0CD3FA024597FD38", "scripts/cp_common/debug.gsc"},
			{"14A23115315744C4", "scripts/cp_common/debug_menu.gsc"},
			{"438358C7BC220601", "scripts/cp_common/devgui.gsc"},
			{"5E5CFD99BD50B21A", "scripts/cp_common/entityheadicons.gsc"},
			{"741CEA6DAC97E194", "scripts/cp_common/friendlyfire.gsc"},
			{"653EB9A685B22D37", "scripts/cp_common/load.gsc"},
			{"50D7B464FFEDC2EF", "scripts/cp_common/objectives.gsc"},
			{"685EF6C0250A1053", "scripts/cp_common/oed.gsc"},
			{"66FBF625A3C2B5A8", "scripts/cp_common/rat.gsc"},
			{"1BCC93F7B3F76B9D", "scripts/cp_common/skipto.gsc"},
			{"15D9188A37CF728C", "scripts/cp_common/spawn_manager.gsc"},
			{"2AAAD701421B20E3", "scripts/cp_common/util.gsc"},
			{"2A59F9BE285A7714", "scripts/cp_common/gametypes/battlechatter.gsc"},
			{"30883855E703365B", "scripts/cp_common/gametypes/deathicons.gsc"},
			{"67BD93385A242074", "scripts/cp_common/gametypes/dev.gsc"},
			{"4026690904ACAE07", "scripts/cp_common/gametypes/dev_class.gsc"},
			{"430F601B51DEB264", "scripts/cp_common/gametypes/globallogic.gsc"},
			{"6CB684BEEBAA6D1E", "scripts/cp_common/gametypes/globallogic_actor.gsc"},
			{"4156769C221B7131", "scripts/cp_common/gametypes/globallogic_defaults.gsc"},
			{"2B02AF9DD46C86F6", "scripts/cp_common/gametypes/globallogic_player.gsc"},
			{"7A74017568B1FDA4", "scripts/cp_common/gametypes/globallogic_spawn.gsc"},
			{"77B0C3F7DE3D7B91", "scripts/cp_common/gametypes/globallogic_ui.gsc"},
			{"4E8681424E548A3A", "scripts/cp_common/gametypes/globallogic_utils.gsc"},
			{"14A780EE337B2D83", "scripts/cp_common/gametypes/globallogic_vehicle.gsc"},
			{"640963A813BCD393", "scripts/cp_common/gametypes/loadout.gsc"},
			{"28967100523895A7", "scripts/cp_common/gametypes/menus.gsc"},
			{"32673AB7037900D9", "scripts/cp_common/gametypes/perplayer.gsc"},
			{"16E6188A50853345", "scripts/cp_common/gametypes/serversettings.gsc"},
			{"6B121442C78CA893", "scripts/cp_common/gametypes/shellshock.gsc"},
			{"382DB69BF279EEF2", "scripts/cp_common/gametypes/spawning.gsc"},
			{"6F112710116E1A50", "scripts/cp_common/gametypes/spawnlogic.gsc"},
			{"184EBE7E363C826E", "scripts/mp_common/arena.gsc"},
			{"74C69DD48E66416C", "scripts/mp_common/art.gsc"},
			{"1211768F02D06A75", "scripts/mp_common/bb.gsc"},
			{"08CE39DCB38227AA", "scripts/mp_common/blackjack_challenges.gsc"},
			{"6DC1591109B78439", "scripts/mp_common/callbacks.gsc"},
			{"36E4ABA95436C83F", "scripts/mp_common/challenges.gsc"},
			{"6BECA81BF4A3EB22", "scripts/mp_common/contracts.gsc"},
			{"3C9BECCE0D2E0ADB", "scripts/mp_common/devgui.gsc"},
			{"0B6E300C57E493FA", "scripts/mp_common/draft.gsc"},
			{"0B1F37D02274AEDC", "scripts/mp_common/entityheadicons.gsc"},
			{"03F1165928BB2760", "scripts/mp_common/events.gsc"},
			{"5842D6D0D5B74086", "scripts/mp_common/gameadvertisement.gsc"},
			{"181AB731E1770BEC", "scripts/mp_common/gamerep.gsc"},
			{"1C8AFB3EA2F4A039", "scripts/mp_common/laststand.gsc"},
			{"494D5EE03F7570DD", "scripts/mp_common/load.gsc"},
			{"159CA76EE2C87CCB", "scripts/mp_common/mgturret.gsc"},
			{"55AE2F7DCE64C92C", "scripts/mp_common/perks.gsc"},
			{"5579DC128B222AF2", "scripts/mp_common/pickup_ammo.gsc"},
			{"0CD46E5EC42C7612", "scripts/mp_common/pickup_health.gsc"},
			{"75D068D4C363EED2", "scripts/mp_common/pickup_items.gsc"},
			{"2358353E9356006A", "scripts/mp_common/rat.gsc"},
			{"581FC86B2119B89E", "scripts/mp_common/scoreevents.gsc"},
			{"7EB723AA77552F12", "scripts/mp_common/spawnbeacon.gsc"},
			{"153655059E241A64", "scripts/mp_common/sprint_boost_grenade.gsc"},
			{"207EBAC70D55ACAD", "scripts/mp_common/tracker.gsc"},
			{"2A00D97E32C629E9", "scripts/mp_common/userspawnselection.gsc"},
			{"06AC2D19409DBE5D", "scripts/mp_common/util.gsc"},
			{"3967B59363B263A1", "scripts/mp_common/vehicle.gsc"},
			{"441BC2554613491E", "scripts/mp_common/gametypes/classicmode.gsc"},
			{"10B33A3ED149B094", "scripts/mp_common/gametypes/clean.gsc"},
			{"5C2CD6A6DEA6246E", "scripts/mp_common/gametypes/clientids.gsc"},
			{"55FEFA9B747A887B", "scripts/mp_common/gametypes/conf.gsc"},
			{"7455205D482DB231", "scripts/mp_common/gametypes/deathicons.gsc"},
			{"0BA5793ED0E9D302", "scripts/mp_common/gametypes/dev.gsc"},
			{"1619DC8E536F67BD", "scripts/mp_common/gametypes/dev_class.gsc"},
			{"23F30FF57B008BFE", "scripts/mp_common/gametypes/dm.gsc"},
			{"630A36049D851329", "scripts/mp_common/gametypes/dom.gsc"},
			{"60A81F1FC0034637", "scripts/mp_common/gametypes/gametype.gsc"},
			{"029DC52DA62A31E6", "scripts/mp_common/gametypes/globallogic.gsc"},
			{"795FF5B40160D8B0", "scripts/mp_common/gametypes/globallogic_actor.gsc"},
			{"66E14949B665E9FB", "scripts/mp_common/gametypes/globallogic_audio.gsc"},
			{"6EEFC77D3C93AC3B", "scripts/mp_common/gametypes/globallogic_defaults.gsc"},
			{"6BFE82328B184B9C", "scripts/mp_common/gametypes/globallogic_player.gsc"},
			{"17186A8269394DBB", "scripts/mp_common/gametypes/globallogic_score.gsc"},
			{"2104AD1148779BBE", "scripts/mp_common/gametypes/globallogic_spawn.gsc"},
			{"7D9BEB5FEA147CFC", "scripts/mp_common/gametypes/globallogic_tickets.gsc"},
			{"525BB00AC83DE47B", "scripts/mp_common/gametypes/globallogic_ui.gsc"},
			{"13EA62F7895ECEE4", "scripts/mp_common/gametypes/globallogic_utils.gsc"},
			{"5A17A6EB846C2B8F", "scripts/mp_common/gametypes/gun.gsc"},
			{"1F49D7B5DE6FFBA7", "scripts/mp_common/gametypes/hostmigration.gsc"},
			{"11264F26F6442384", "scripts/mp_common/gametypes/hud_message.gsc"},
			{"67BF0CD995283AFB", "scripts/mp_common/gametypes/koth.gsc"},
			{"0448DF5469C10A4C", "scripts/mp_common/gametypes/match.gsc"},
			{"21C9539C615512A9", "scripts/mp_common/gametypes/menus.gsc"},
			{"46009A5996D2C4FF", "scripts/mp_common/gametypes/os.gsc"},
			{"60EDE3A57859FC00", "scripts/mp_common/gametypes/osdm.gsc"},
			{"0B1AF04588DD5A30", "scripts/mp_common/gametypes/ostdm.gsc"},
			{"602435751A83D601", "scripts/mp_common/gametypes/outcome.gsc"},
			{"19731285EC1D6806", "scripts/mp_common/gametypes/overtime.gsc"},
			{"515840075A0D467B", "scripts/mp_common/gametypes/perplayer.gsc"},
			{"247A47CCEF42700C", "scripts/mp_common/gametypes/prop.gsc"},
			{"3860849AD74F6565", "scripts/mp_common/gametypes/round.gsc"},
			{"1561B416AB9A32D2", "scripts/mp_common/gametypes/sd.gsc"},
			{"52650CB1FD84A72B", "scripts/mp_common/gametypes/serversettings.gsc"},
			{"6D037A49E4710BBD", "scripts/mp_common/gametypes/shellshock.gsc"},
			{"10CBA039F3D1F644", "scripts/mp_common/gametypes/spawning.gsc"},
			{"365F8B2483CA721E", "scripts/mp_common/gametypes/spawnlogic.gsc"},
			{"03D5BDD30FE3EC4A", "scripts/mp_common/gametypes/tdm.gsc"},
			{"5140126A636B0A1F", "scripts/mp_common/gametypes/war.gsc"},
			{"668ABA6EE6D1FB9A", "scripts/mp_common/gametypes/_prop_controls.gsc"},
			{"24A41F95ADA40341", "scripts/mp_common/gametypes/_prop_dev.gsc"},
			{"484E6A6145005870", "scripts/wz_common/hud.gsc"},
			{"4967DFC092221A25", "scripts/wz_common/oob.gsc"},
			{"74DBB72B4083A104", "scripts/wz_common/player.gsc"},
			{"44A09BF034E42485", "scripts/wz_common/util.gsc"},
			{"028BC78B124E9029", "scripts/wz_common/vehicle.gsc"},
			{"1266C848683FFF8C", "scripts/wz_common/gametypes/spawning.gsc"},
			{"049143E3C792C475", "scripts/zm/zm_ai_raz.gsc"},
			{"4F598E8DD6581D57", "scripts/zm/zm_gold.gsc"},
			{"3A971F381A677DA5", "scripts/zm/zm_gold_ffotd.gsc"},
			{"5E1D963D10405D96", "scripts/zm/zm_gold_gamemodes.gsc"},
			{"5E63D8C78D003976", "scripts/zm/zm_gold_main_quest.gsc"},
			{"2724A2F18F235914", "scripts/zm/zm_gold_pap_quest.gsc"},
			{"04CEA20DC230327E", "scripts/zm/zm_gold_util.gsc"},
			{"1241BE8E5F2D28A5", "scripts/zm/zm_gold_ww_quest.gsc"},
			{"6374BAC2C0C999F7", "scripts/zm/zm_gold_zones.gsc"},
			{"397AFBC4CD0B7210", "scripts/zm/zm_silver.gsc"},
			{"7864400788216CFA", "scripts/zm/zm_silver_ffotd.gsc"},
			{"60DE9B7780C971D9", "scripts/zm/zm_silver_gamemodes.gsc"},
			{"10B6198242913C7F", "scripts/zm/zm_silver_main_quest.gsc"},
			{"118CA25348A7A86B", "scripts/zm/zm_silver_pap_quest.gsc"},
			{"6E79ABFE0BC16FC8", "scripts/zm/zm_silver_sound.gsc"},
			{"55AD4CB067E88CBF", "scripts/zm/zm_silver_util.gsc"},
			{"49AF00B5F42FAF14", "scripts/zm/zm_silver_ww_quest.gsc"},
			{"60FA0BE43CE12C54", "scripts/zm/zm_silver_zones.gsc"},
			{"6B7F69DD658D5E98", "scripts/zm_common/art.gsc"},
			{"7099E4031FCCE7CC", "scripts/zm_common/ballistic_knife.gsc"},
			{"5D1A60FAEC6DD721", "scripts/zm_common/bb.gsc"},
			{"405148B9E199F22D", "scripts/zm_common/callbacks.gsc"},
			{"7869A939BAD01B3B", "scripts/zm_common/fx.gsc"},
			{"5F85E82374C52031", "scripts/zm_common/load.gsc"},
			{"407D5809E1BDDED6", "scripts/zm_common/rat.gsc"},
			{"532DDA0584E11D5A", "scripts/zm_common/scoreevents.gsc"},
			{"5DDD49D6268A02B1", "scripts/zm_common/util.gsc"},
			{"19F8E3DCF8B6367C", "scripts/zm_common/zm.gsc"},
			{"3505DDB734AF5182", "scripts/zm_common/zm_ai_faller.gsc"},
			{"1A3CAA74B0692754", "scripts/zm_common/zm_altbody.gsc"},
			{"20F6685AD6B69636", "scripts/zm_common/zm_attackables.gsc"},
			{"1E448A29478C01E1", "scripts/zm_common/zm_audio.gsc"},
			{"26B560B6EEDDED39", "scripts/zm_common/zm_behavior.gsc"},
			{"6BBA029AE7C4A086", "scripts/zm_common/zm_behavior_utility.gsc"},
			{"609DB10160D26D08", "scripts/zm_common/zm_bgb.gsc"},
			{"14DD21D296D67DB0", "scripts/zm_common/zm_blockers.gsc"},
			{"371C3A894B4A77D5", "scripts/zm_common/zm_challenges.gsc"},
			{"661D141FAF0648BA", "scripts/zm_common/zm_clone.gsc"},
			{"1758E81D9BA01AB3", "scripts/zm_common/zm_daily_challenges.gsc"},
			{"79BAAD7B770E1731", "scripts/zm_common/zm_devgui.gsc"},
			{"4888B176F495EA4F", "scripts/zm_common/zm_equipment.gsc"},
			{"7C7BE2FD315F25E6", "scripts/zm_common/zm_ffotd.gsc"},
			{"214CC29560C6BF98", "scripts/zm_common/zm_game_module.gsc"},
			{"5CE6F2B150FA3C97", "scripts/zm_common/zm_game_module_utility.gsc"},
			{"32808E3E5B921CC0", "scripts/zm_common/zm_grappler.gsc"},
			{"376229323ED0E5F5", "scripts/zm_common/zm_jump_pad.gsc"},
			{"38535AEB9E457F37", "scripts/zm_common/zm_laststand.gsc"},
			{"4A46360C73164083", "scripts/zm_common/zm_magicbox.gsc"},
			{"184F27416CB486E2", "scripts/zm_common/zm_melee_weapon.gsc"},
			{"680310C0CDF0855D", "scripts/zm_common/zm_mgturret.gsc"},
			{"3147F804B1776A38", "scripts/zm_common/zm_net.gsc"},
			{"2C23F5A5240ECE35", "scripts/zm_common/zm_pack_a_punch.gsc"},
			{"20944AF40937DDD4", "scripts/zm_common/zm_pack_a_punch_util.gsc"},
			{"11E13B02DFA60F4A", "scripts/zm_common/zm_perks.gsc"},
			{"4066C93D696A5698", "scripts/zm_common/zm_placeable_mine.gsc"},
			{"7C090196DD6EAD2E", "scripts/zm_common/zm_player.gsc"},
			{"6C566DCAFBD24E16", "scripts/zm_common/zm_power.gsc"},
			{"13DAC574989A9C42", "scripts/zm_common/zm_powerups.gsc"},
			{"142660BE9C72328D", "scripts/zm_common/zm_puppet.gsc"},
			{"022F61D3553FDEED", "scripts/zm_common/zm_score.gsc"},
			{"62175F6BFEB32F3F", "scripts/zm_common/zm_server_throttle.gsc"},
			{"15F8D126EF6022DF", "scripts/zm_common/zm_spawner.gsc"},
			{"0D27C987C60BD6E4", "scripts/zm_common/zm_stats.gsc"},
			{"01DD45501CBE325B", "scripts/zm_common/zm_traps.gsc"},
			{"74D98A95CEB996FD", "scripts/zm_common/zm_turned.gsc"},
			{"4506927A5DCEF7F9", "scripts/zm_common/zm_unitrigger.gsc"},
			{"056ED1E844148A53", "scripts/zm_common/zm_utility.gsc"},
			{"540ADBBB36B28DF6", "scripts/zm_common/zm_weapons.gsc"},
			{"7CB088E2E41CBC0C", "scripts/zm_common/zm_weap_bouncingbetty.gsc"},
			{"2A64BACDECE90897", "scripts/zm_common/zm_zonemgr.gsc"},
			{"510A7C58311CD7B5", "scripts/zm_common/bgbs/zm_bgb_newtonian_negation.gsc"},
			{"0062CFE4D7BC5F6A", "scripts/zm_common/gametypes/clientids.gsc"},
			{"3758A7A305B9F99E", "scripts/zm_common/gametypes/dev.gsc"},
			{"127470A42D23E8F9", "scripts/zm_common/gametypes/dev_class.gsc"},
			{"11300BEBD5FA60B2", "scripts/zm_common/gametypes/globallogic.gsc"},
			{"225F55D583E25424", "scripts/zm_common/gametypes/globallogic_actor.gsc"},
			{"06FE720B11766967", "scripts/zm_common/gametypes/globallogic_audio.gsc"},
			{"291D7F2941A107F7", "scripts/zm_common/gametypes/globallogic_defaults.gsc"},
			{"01669731DA8FF9B8", "scripts/zm_common/gametypes/globallogic_player.gsc"},
			{"3676749786F70D5F", "scripts/zm_common/gametypes/globallogic_score.gsc"},
			{"70C2381177705442", "scripts/zm_common/gametypes/globallogic_spawn.gsc"},
			{"2C785B2B9BB1A88F", "scripts/zm_common/gametypes/globallogic_ui.gsc"},
			{"457B9FFC450B8F80", "scripts/zm_common/gametypes/globallogic_utils.gsc"},
			{"1D7356B0B8791E93", "scripts/zm_common/gametypes/hostmigration.gsc"},
			{"480B48267E979D20", "scripts/zm_common/gametypes/hud_message.gsc"},
			{"10885AFB593D7587", "scripts/zm_common/gametypes/perplayer.gsc"},
			{"18F6E2EA47D67E57", "scripts/zm_common/gametypes/serversettings.gsc"},
			{"5FE0D56BD24FD639", "scripts/zm_common/gametypes/shellshock.gsc"},
			{"3044059EDFD97240", "scripts/zm_common/gametypes/spawning.gsc"},
			{"50FC1AF939D4A6D2", "scripts/zm_common/gametypes/spawnlogic.gsc"},
			{"57D39B94F77E0AC9", "scripts/zm_common/gametypes/spectating.gsc"},
			{"5AA2E67B4E9D71CB", "scripts/zm_common/gametypes/zclassic.gsc"},
			{"364E95A7497BBBAC", "scripts/zm_common/gametypes/zgrief.gsc"},
			{"2B95C1A766082511", "scripts/zm_common/gametypes/zm_gametype.gsc"},
			{"6864BC7F8CF9C6AE", "scripts/zm_common/gametypes/zstandard.gsc"},
			{"77D579CE6AB5890A", "scripts/zm_common/gametypes/ztrials.gsc"},
			{"3273B135D0859C51", "scripts/zm_common/gametypes/ztutorial.gsc"},
		};
		public static string attemptNameForHash(string hash)
		{
			return knownStringsMap.ContainsKey(hash) ? knownStringsMap[hash] : hash;
		}
	}
}
