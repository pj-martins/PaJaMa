using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

// TODO: copied code... cleanup
namespace PaJaMa.DatabaseStudio.Monitor
{
	public partial class ucMonitor : UserControl
	{
		private RawTraceReader _reader;
		private Queue<ProfilerEvent> _eventQueue = new Queue<ProfilerEvent>(10);
		private Thread _tracerThread;
		private bool _stopRequested = true;

		private List<ProfilerEvent> _events = new List<ProfilerEvent>();
		private BindingList<ProfilerEvent> _filtered = new BindingList<ProfilerEvent>();

		public ucMonitor()
		{
			InitializeComponent();

			if (Properties.Settings.Default.MonitorConnectionStrings == null)
				Properties.Settings.Default.MonitorConnectionStrings = string.Empty;

			refreshConnStrings();

			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastMonitorConnectionString))
				cboConnectionString.Text = Properties.Settings.Default.LastMonitorConnectionString;

			gridResults.AutoGenerateColumns = false;
			gridResults.DataSource = _filtered;
		}

		private void refreshConnStrings()
		{
			var conns = Properties.Settings.Default.MonitorConnectionStrings.Split('|');
			cboConnectionString.Items.Clear();
			cboConnectionString.Items.AddRange(conns.OrderBy(c => c).ToArray());
		}

		private void ucMonitor_Load(object sender, EventArgs e)
		{
		}

		private bool startProfiling()
		{
			try
			{
				_reader = new RawTraceReader(cboConnectionString.Text);
				_reader.CreateTrace();
				if (true)
				{
					//if (m_currentsettings.EventsColumns.LoginLogout)
					{
						//m_Rdr.SetEvent(ProfilerEvents.SecurityAudit.AuditLogin,
						//			   ProfilerEventColumns.TextData,
						//			   ProfilerEventColumns.LoginName,
						//			   ProfilerEventColumns.SPID,
						//			   ProfilerEventColumns.StartTime,
						//			   ProfilerEventColumns.EndTime,
						//			   ProfilerEventColumns.HostName
						//	);
						//m_Rdr.SetEvent(ProfilerEvents.SecurityAudit.AuditLogout,
						//			   ProfilerEventColumns.CPU,
						//			   ProfilerEventColumns.Reads,
						//			   ProfilerEventColumns.Writes,
						//			   ProfilerEventColumns.Duration,
						//			   ProfilerEventColumns.LoginName,
						//			   ProfilerEventColumns.SPID,
						//			   ProfilerEventColumns.StartTime,
						//			   ProfilerEventColumns.EndTime,
						//			   ProfilerEventColumns.ApplicationName,
						//			   ProfilerEventColumns.HostName
						//	);
					}

					//if (m_currentsettings.EventsColumns.ExistingConnection)
					{
						//m_Rdr.SetEvent(ProfilerEvents.Sessions.ExistingConnection,
						//			   ProfilerEventColumns.TextData,
						//			   ProfilerEventColumns.SPID,
						//			   ProfilerEventColumns.StartTime,
						//			   ProfilerEventColumns.EndTime,
						//			   ProfilerEventColumns.ApplicationName,
						//			   ProfilerEventColumns.HostName
						//	);
					}
					//if (m_currentsettings.EventsColumns.BatchCompleted)
					{
						_reader.SetEvent(ProfilerEvents.TSQL.SQLBatchCompleted,
									   ProfilerEventColumns.TextData,
									   ProfilerEventColumns.LoginName,
									   ProfilerEventColumns.CPU,
									   ProfilerEventColumns.Reads,
									   ProfilerEventColumns.Writes,
									   ProfilerEventColumns.Duration,
									   ProfilerEventColumns.SPID,
									   ProfilerEventColumns.StartTime,
									   ProfilerEventColumns.EndTime,
									   ProfilerEventColumns.DatabaseName,
									   ProfilerEventColumns.ApplicationName,
									   ProfilerEventColumns.HostName
							);
					}
					//if (m_currentsettings.EventsColumns.BatchStarting)
					{
						//m_Rdr.SetEvent(ProfilerEvents.TSQL.SQLBatchStarting,
						//			   ProfilerEventColumns.TextData,
						//			   ProfilerEventColumns.LoginName,
						//			   ProfilerEventColumns.SPID,
						//			   ProfilerEventColumns.StartTime,
						//			   ProfilerEventColumns.EndTime,
						//			   ProfilerEventColumns.DatabaseName,
						//			   ProfilerEventColumns.ApplicationName,
						//			   ProfilerEventColumns.HostName
						//	);
					}
					//if (m_currentsettings.EventsColumns.RPCStarting)
					{
						//m_Rdr.SetEvent(ProfilerEvents.StoredProcedures.RPCStarting,
						//			   ProfilerEventColumns.TextData,
						//			   ProfilerEventColumns.LoginName,
						//			   ProfilerEventColumns.SPID,
						//			   ProfilerEventColumns.StartTime,
						//			   ProfilerEventColumns.EndTime,
						//			   ProfilerEventColumns.DatabaseName,
						//			   ProfilerEventColumns.ObjectName,
						//			   ProfilerEventColumns.ApplicationName,
						//			   ProfilerEventColumns.HostName

						//	);
					}

				}
				//if (m_currentsettings.EventsColumns.RPCCompleted)
				{
					_reader.SetEvent(ProfilerEvents.StoredProcedures.RPCCompleted,
								   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
								   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
								   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
								   ProfilerEventColumns.SPID
								   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
								   , ProfilerEventColumns.DatabaseName
								   , ProfilerEventColumns.ObjectName
								   , ProfilerEventColumns.ApplicationName
								   , ProfilerEventColumns.HostName

						);
				}
				//if (m_currentsettings.EventsColumns.SPStmtCompleted)
				{
					//m_Rdr.SetEvent(ProfilerEvents.StoredProcedures.SPStmtCompleted,
					//			   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
					//			   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
					//			   ProfilerEventColumns.SPID
					//			   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
					//			   , ProfilerEventColumns.DatabaseName
					//			   , ProfilerEventColumns.ObjectName
					//			   , ProfilerEventColumns.ObjectID
					//			   , ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//);
				}
				//if (m_currentsettings.EventsColumns.SPStmtStarting)
				{
					//m_Rdr.SetEvent(ProfilerEvents.StoredProcedures.SPStmtStarting,
					//			   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
					//			   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
					//			   ProfilerEventColumns.SPID
					//			   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
					//			   , ProfilerEventColumns.DatabaseName
					//			   , ProfilerEventColumns.ObjectName
					//			   , ProfilerEventColumns.ObjectID
					//			   , ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//);
				}
				//if (m_currentsettings.EventsColumns.UserErrorMessage)
				{
					//m_Rdr.SetEvent(ProfilerEvents.ErrorsAndWarnings.UserErrorMessage,
					//			   ProfilerEventColumns.TextData,
					//			   ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU,
					//			   ProfilerEventColumns.SPID,
					//			   ProfilerEventColumns.StartTime,
					//			   ProfilerEventColumns.DatabaseName,
					//			   ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//	);
				}
				//if (m_currentsettings.EventsColumns.BlockedProcessPeport)
				{
					//m_Rdr.SetEvent(ProfilerEvents.ErrorsAndWarnings.Blockedprocessreport,
					//			   ProfilerEventColumns.TextData,
					//			   ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU,
					//			   ProfilerEventColumns.SPID,
					//			   ProfilerEventColumns.StartTime,
					//			   ProfilerEventColumns.DatabaseName,
					//			   ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//	);

				}

				//if (m_currentsettings.EventsColumns.SQLStmtStarting)
				{
					//m_Rdr.SetEvent(ProfilerEvents.TSQL.SQLStmtStarting,
					//			   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
					//			   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
					//			   ProfilerEventColumns.SPID
					//			   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
					//			   , ProfilerEventColumns.DatabaseName
					//			   , ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//	);
				}
				//if (m_currentsettings.EventsColumns.SQLStmtCompleted)
				{
					//m_Rdr.SetEvent(ProfilerEvents.TSQL.SQLStmtCompleted,
					//			   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
					//			   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
					//			   ProfilerEventColumns.SPID
					//			   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
					//			   , ProfilerEventColumns.DatabaseName
					//			   , ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName
					//	);
				}

				//if (null != m_currentsettings.Filters.Duration)
				//{
				//	SetIntFilter(m_currentsettings.Filters.Duration * 1000,
				//				 m_currentsettings.Filters.DurationFilterCondition, ProfilerEventColumns.Duration);
				//}
				//SetIntFilter(m_currentsettings.Filters.Reads, m_currentsettings.Filters.ReadsFilterCondition, ProfilerEventColumns.Reads);
				//SetIntFilter(m_currentsettings.Filters.Writes, m_currentsettings.Filters.WritesFilterCondition, ProfilerEventColumns.Writes);
				//SetIntFilter(m_currentsettings.Filters.CPU, m_currentsettings.Filters.CpuFilterCondition, ProfilerEventColumns.CPU);
				//SetIntFilter(m_currentsettings.Filters.SPID, m_currentsettings.Filters.SPIDFilterCondition, ProfilerEventColumns.SPID);

				//SetStringFilter(m_currentsettings.Filters.LoginName, m_currentsettings.Filters.LoginNameFilterCondition, ProfilerEventColumns.LoginName);
				//SetStringFilter(m_currentsettings.Filters.HostName, m_currentsettings.Filters.HostNameFilterCondition, ProfilerEventColumns.HostName);
				//SetStringFilter(m_currentsettings.Filters.DatabaseName, m_currentsettings.Filters.DatabaseNameFilterCondition, ProfilerEventColumns.DatabaseName);
				//SetStringFilter(m_currentsettings.Filters.TextData, m_currentsettings.Filters.TextDataFilterCondition, ProfilerEventColumns.TextData);
				//SetStringFilter(m_currentsettings.Filters.ApplicationName, m_currentsettings.Filters.ApplicationNameFilterCondition, ProfilerEventColumns.ApplicationName);


				_reader.SetFilter(ProfilerEventColumns.ApplicationName, LogicalOperators.AND, ComparisonOperators.NotLike,
								"PaJaMa Database Studio");
				_reader.SetFilter(ProfilerEventColumns.TextData, LogicalOperators.AND, ComparisonOperators.NotEqual,
					"exec sp_reset_connection");
				_eventQueue.Clear();
				startProfilerThread();
				timer1.Enabled = true;
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private void stopProfiling()
		{
			timer1.Stop();
			_reader.StopTrace();
			_stopRequested = true;
			if (_tracerThread.IsAlive)
			{
				_tracerThread.Abort();
			}
			_tracerThread.Join();
		}

		private void startProfilerThread()
		{
			if (_reader != null)
			{
				_reader.Close();
			}
			_reader.StartTrace();
			_tracerThread = new Thread(profilerThread) { IsBackground = true, Priority = ThreadPriority.Lowest };
			_stopRequested = false;
			_tracerThread.Start();
		}

		private void profilerThread(Object state)
		{
			try
			{
				while (!_stopRequested && _reader.TraceIsActive)
				{
					ProfilerEvent evt = _reader.Next();
					if (evt != null)
					{
						lock (this)
						{
							_eventQueue.Enqueue(evt);
						}
					}
				}
			}
			catch (Exception e)
			{
			}
		}

		private string getEventCaption(ProfilerEvent evt)
		{
			return ProfilerEvents.Names[evt.EventClass];
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Queue<ProfilerEvent> saved;
			lock (this)
			{
				saved = _eventQueue;
				_eventQueue = new Queue<ProfilerEvent>(10);
			}
			while (0 != saved.Count)
			{
				NewEventArrived(saved.Dequeue(), 0 == saved.Count);
			}
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (startProfiling())
			{
				List<string> connStrings = Properties.Settings.Default.MonitorConnectionStrings.Split('|').ToList();
				if (!connStrings.Any(s => s == cboConnectionString.Text))
					connStrings.Add(cboConnectionString.Text);

				Properties.Settings.Default.MonitorConnectionStrings = string.Join("|", connStrings.ToArray());
				Properties.Settings.Default.LastMonitorConnectionString = cboConnectionString.Text;
				Properties.Settings.Default.Save();

				btnConnect.Visible = btnRemoveConnString.Visible = false;
				btnDisconnect.Visible = true;
				cboConnectionString.SelectionLength = 0;
				cboConnectionString.Enabled = false;
			}
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			stopProfiling();

			btnConnect.Visible = btnRemoveConnString.Visible = true;
			btnDisconnect.Visible = false;
			cboConnectionString.Enabled = true;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			_events.Clear();
			_filtered.Clear();
			chkApplication.CheckBoxItems.Clear();
			chkLogin.CheckBoxItems.Clear();
		}

		private void checkFilterBoxItems()
		{
			var distinctApplications = _events.Select(e => e.ApplicationName.Trim()).Distinct();
			foreach (var a in distinctApplications)
			{
				if (!chkApplication.Items.OfType<string>().Any(s => s == a))
					chkApplication.Items.Add(a);
			}

			var distinctLogins = _events.Select(e => e.LoginName.Trim()).Distinct();
			foreach (var l in distinctLogins)
			{
				if (!chkLogin.Items.OfType<string>().Any(s => s == l))
					chkLogin.Items.Add(l);
			}
		}

		private void addFilterEvent(ProfilerEvent evt)
		{
			bool add = true;
			if (chkApplication.CheckBoxItems.Any(i => i.Checked)
				&& !chkApplication.CheckBoxItems.Any(i => i.Checked && i.ComboBoxItem.ToString().Trim() == evt.ApplicationName.Trim()))
				add = false;

			if (chkLogin.CheckBoxItems.Any(i => i.Checked)
				&& !chkLogin.CheckBoxItems.Any(i => i.Checked && i.ComboBoxItem.ToString().Trim() == evt.LoginName.Trim()))
				add = false;

			if (add)
				_filtered.Add(evt);
		}

		private void applyFilter()
		{
			_filtered.Clear();
			foreach (var evt in _events)
			{
				addFilterEvent(evt);
			}
		}

		private void chkApplication_CheckBoxCheckedChanged(object sender, EventArgs e)
		{
			applyFilter();
		}

		private void chkLogin_CheckBoxCheckedChanged(object sender, EventArgs e)
		{
			applyFilter();
		}

		private void NewEventArrived(ProfilerEvent evt, bool last)
		{
			if (string.IsNullOrEmpty(evt.TextData)) return;
			_events.Add(evt);
			addFilterEvent(evt);
			checkFilterBoxItems();
		}

		private void gridResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == colTextData.Index && e.RowIndex >= 0)
			{
				var frm = new frmQueryText();
				frm.txtQuery.Text = gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
				frm.Show();
				frm.txtQuery.SelectionLength = 0;
			}
		}
	}
}
