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
using System.Data.SqlClient;

// TODO: copied code... cleanup
namespace PaJaMa.DatabaseStudio.Monitor
{
	public partial class ucMonitor : UserControl
	{
		private class PerfInfo
		{
			internal int m_count;
			internal readonly DateTime m_date = DateTime.Now;
		}

		private SqlConnection m_Conn;
		private readonly SqlCommand m_Cmd = new SqlCommand();
		private readonly Queue<PerfInfo> m_perf = new Queue<PerfInfo>();
		private PerfInfo m_first, m_prev;
		//internal TraceSettings m_currentsettings;
		private enum ProfilingStateEnum { psStopped, psProfiling, psPaused }
		private ProfilingStateEnum m_ProfilingState;
		private int m_EventCount;
		private RawTraceReader m_Rdr;
		Queue<ProfilerEvent> m_events = new Queue<ProfilerEvent>(10);
		private Thread m_Thr;
		private bool m_NeedStop = true;
		private readonly ProfilerEvent m_EventStarted = new ProfilerEvent();
		private readonly ProfilerEvent m_EventStopped = new ProfilerEvent();
		private readonly ProfilerEvent m_EventPaused = new ProfilerEvent();

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
			//string connString = "server=m4dev;database=EForms;trusted_connection=yes";
			//_tracer = new Tracer(connString);
			//_tracer.CreateTrace();

			//_tracer.SetEvent(ProfilerEvents.TSQL.SQLBatchCompleted,
			//						   ProfilerEventColumns.TextData,
			//						   ProfilerEventColumns.LoginName,
			//						   ProfilerEventColumns.CPU,
			//						   ProfilerEventColumns.Reads,
			//						   ProfilerEventColumns.Writes,
			//						   ProfilerEventColumns.Duration,
			//						   ProfilerEventColumns.SPID,
			//						   ProfilerEventColumns.StartTime,
			//						   ProfilerEventColumns.EndTime,
			//						   ProfilerEventColumns.DatabaseName,
			//						   ProfilerEventColumns.ApplicationName,
			//						   ProfilerEventColumns.HostName
			//				);

			//startProfiler();
			//StartProfiling();
		}

		//private void startProfiler()
		//{
		//	_tracer.StartTrace();
		//	// m_ProfilingState = ProfilingStateEnum.psProfiling;
		//	bwTrace.RunWorkerAsync();
		//}

		//private void bwTracer_DoWork(object sender, DoWorkEventArgs e)
		//{
		//	while (!bwTrace.CancellationPending)
		//	{
		//		var evt = _tracer.Next();
		//		if (evt != null)
		//		{

		//		}
		//	}
		//}

		private bool StartProfiling()
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				m_perf.Clear();
				m_first = null;
				m_prev = null;
				if (m_ProfilingState == ProfilingStateEnum.psPaused)
				{
					// ResumeProfiling();
					return false;
				}
				if (m_Conn != null && m_Conn.State == ConnectionState.Open)
				{
					m_Conn.Close();
				}
				//InitGridColumns();
				m_EventCount = 0;
				m_Conn = GetConnection();
				m_Conn.Open();
				m_Rdr = new RawTraceReader(m_Conn);

				m_Rdr.CreateTrace();
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
						m_Rdr.SetEvent(ProfilerEvents.TSQL.SQLBatchCompleted,
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
					//m_Rdr.SetEvent(ProfilerEvents.StoredProcedures.RPCCompleted,
					//			   ProfilerEventColumns.TextData, ProfilerEventColumns.LoginName,
					//			   ProfilerEventColumns.CPU, ProfilerEventColumns.Reads,
					//			   ProfilerEventColumns.Writes, ProfilerEventColumns.Duration,
					//			   ProfilerEventColumns.SPID
					//			   , ProfilerEventColumns.StartTime, ProfilerEventColumns.EndTime
					//			   , ProfilerEventColumns.DatabaseName
					//			   , ProfilerEventColumns.ObjectName
					//			   , ProfilerEventColumns.ApplicationName
					//			   , ProfilerEventColumns.HostName

					//	);
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


				m_Cmd.Connection = m_Conn;
				m_Cmd.CommandTimeout = 0;
				m_Rdr.SetFilter(ProfilerEventColumns.ApplicationName, LogicalOperators.AND, ComparisonOperators.NotLike,
								"Express Profiler");
				//m_Cached.Clear();
				m_events.Clear();
				//m_itembysql.Clear();
				//lvEvents.VirtualListSize = 0;
				StartProfilerThread();
				//m_servername = edServer.Text;
				//m_username = edUser.Text;
				//SaveDefaultSettings();
				timer1.Enabled = true;
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			finally
			{
				//UpdateButtons();
				Cursor = Cursors.Default;
			}
		}

		private void StopProfiling()
		{
			timer1.Stop();
			// tbStop.Enabled = false;
			using (SqlConnection cn = GetConnection())
			{
				cn.Open();
				m_Rdr.StopTrace(cn);
				m_Rdr.CloseTrace(cn);
				cn.Close();
			}
			m_NeedStop = true;
			if (m_Thr.IsAlive)
			{
				m_Thr.Abort();
			}
			m_Thr.Join();
			m_ProfilingState = ProfilingStateEnum.psStopped;
			m_Conn.Close();
			//NewEventArrived(m_EventStopped, true);
			//UpdateButtons();
		}

		private void SetIntFilter(int? value, IntFilterCondition condition, int column)
		{
			int[] com = new[] { ComparisonOperators.Equal, ComparisonOperators.NotEqual, ComparisonOperators.GreaterThan, ComparisonOperators.LessThan };
			if ((null != value))
			{
				long? v = value;
				m_Rdr.SetFilter(column, LogicalOperators.AND, com[(int)condition], v);
			}
		}

		private void SetStringFilter(string value, StringFilterCondition condition, int column)
		{
			if (!String.IsNullOrEmpty(value))
			{
				m_Rdr.SetFilter(column, LogicalOperators.AND
					, condition == StringFilterCondition.Like ? ComparisonOperators.Like : ComparisonOperators.NotLike
					, value
					);
			}

		}

		private SqlConnection GetConnection()
		{
			return new SqlConnection
			{
				ConnectionString = cboConnectionString.Text
				//tbAuth.SelectedIndex == 0 ? String.Format(@"Data Source = {0}; Initial Catalog = master; Integrated Security=SSPI;Application Name=Express Profiler", edServer.Text)
				//: String.Format(@"Data Source={0};Initial Catalog=master;User Id={1};Password='{2}';;Application Name=Express Profiler", edServer.Text, edUser.Text, edPassword.Text)
				//"server=m4dev;database=EForms;trusted_connection=yes"
			};
		}

		private void StartProfilerThread()
		{
			if (m_Rdr != null)
			{
				m_Rdr.Close();
			}
			m_Rdr.StartTrace();
			m_Thr = new Thread(ProfilerThread) { IsBackground = true, Priority = ThreadPriority.Lowest };
			m_NeedStop = false;
			m_ProfilingState = ProfilingStateEnum.psProfiling;
			// NewEventArrived(m_EventStarted, true);
			m_Thr.Start();
		}

		private void ProfilerThread(Object state)
		{
			try
			{
				while (!m_NeedStop && m_Rdr.TraceIsActive)
				{
					ProfilerEvent evt = m_Rdr.Next();
					if (evt != null)
					{
						lock (this)
						{
							m_events.Enqueue(evt);
						}
					}
				}
			}
			catch (Exception e)
			{
				lock (this)
				{
					if (!m_NeedStop && m_Rdr.TraceIsActive)
					{
						//m_profilerexception = e;
					}
				}
			}
		}

		private string GetEventCaption(ProfilerEvent evt)
		{
			if (evt == m_EventStarted)
			{
				return "Trace started";
			}
			if (evt == m_EventPaused)
			{
				return "Trace paused";
			}
			if (evt == m_EventStopped)
			{
				return "Trace stopped";
			}
			return ProfilerEvents.Names[evt.EventClass];
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Queue<ProfilerEvent> saved;
			Exception exc;
			lock (this)
			{
				saved = m_events;
				m_events = new Queue<ProfilerEvent>(10);
				//exc = m_profilerexception;
				//m_profilerexception = null;
			}
			//if (null != exc)
			//{
			//	using (ThreadExceptionDialog dlg = new ThreadExceptionDialog(exc))
			//	{
			//		dlg.ShowDialog();
			//	}
			//}
			//lock (m_Cached)
			//{
			while (0 != saved.Count)
			{
				NewEventArrived(saved.Dequeue(), 0 == saved.Count);
			}
			//if (m_Cached.Count > m_currentsettings.Filters.MaximumEventCount)
			//{
			//	while (m_Cached.Count > m_currentsettings.Filters.MaximumEventCount)
			//	{
			//		m_Cached.RemoveAt(0);
			//	}
			//	lvEvents.VirtualListSize = m_Cached.Count;
			//	lvEvents.Invalidate();
			//}

			if ((null == m_prev) || (DateTime.Now.Subtract(m_prev.m_date).TotalSeconds >= 1))
			{
				PerfInfo curr = new PerfInfo { m_count = m_EventCount };
				if (m_perf.Count >= 60)
				{
					m_first = m_perf.Dequeue();
				}
				if (null == m_first) m_first = curr;
				if (null == m_prev) m_prev = curr;

				DateTime now = DateTime.Now;
				double d1 = now.Subtract(m_prev.m_date).TotalSeconds;
				double d2 = now.Subtract(m_first.m_date).TotalSeconds;
				//slEPS.Text = String.Format("{0} / {1} EPS(last/avg for {2} second(s))",
				//	(Math.Abs(d1 - 0) > 0.001 ? ((curr.m_count - m_prev.m_count) / d1).ToString("#,0.00") : ""),
				//			 (Math.Abs(d2 - 0) > 0.001 ? ((curr.m_count - m_first.m_count) / d2).ToString("#,0.00") : ""), d2.ToString("0"));

				m_perf.Enqueue(curr);
				m_prev = curr;
			}

			//}
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			if (StartProfiling())
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
			StopProfiling();

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
			{
				// ListViewItem current = (lvEvents.SelectedIndices.Count > 0) ? m_Cached[lvEvents.SelectedIndices[0]] : null;
				m_EventCount++;
				string caption = GetEventCaption(evt);
				_events.Add(evt);
				addFilterEvent(evt);
				checkFilterBoxItems();

				//ListViewItem lvi = new ListViewItem(caption);
				//string[] items = new string[m_columns.Count];
				//for (int i = 1; i < m_columns.Count; i++)
				//{
				//	PerfColumn pc = m_columns[i];
				//	items[i - 1] = pc.Column == -1 ? m_EventCount.ToString("#,0") : GetFormattedValue(evt, pc.Column, pc.Format) ?? "";
				//}
				//lvi.SubItems.AddRange(items);
				//lvi.Tag = evt;
				//m_Cached.Add(lvi);
				//if (last)
				//{
				//	lvEvents.VirtualListSize = m_Cached.Count;
				//	lvEvents.SelectedIndices.Clear();
				//	FocusLVI(tbScroll.Checked ? lvEvents.Items[m_Cached.Count - 1] : current, tbScroll.Checked);
				//	lvEvents.Invalidate(lvi.Bounds);
				//}
			}
		}
	}
}
