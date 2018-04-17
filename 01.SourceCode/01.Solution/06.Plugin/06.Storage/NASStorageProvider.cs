using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plugin.Storage
{
    /// <summary>
    /// NAS存储支撑
    /// </summary>
    public class NASStorageProvider : IDisposable
    {
        #region win32 API
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername,
                                             string lpszDomain,
                                             string lpszPassword,
                                             int dwLogonType,
                                             int dwLogonProvider,
                                             ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr existingTokenHandle,
                                                 int SECURITY_IMPERSONATION_LEVEL,
                                                 ref IntPtr duplicateTokenHandle);


        // logon types
        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_LOGON_NETWORK = 3;
        const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        // logon providers
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_PROVIDER_WINNT50 = 3;
        const int LOGON32_PROVIDER_WINNT40 = 2;
        const int LOGON32_PROVIDER_WINNT35 = 1;

        WindowsIdentity newIdentity;
        WindowsImpersonationContext impersonatedUser;
        bool isSuccess = false;
        IntPtr token = IntPtr.Zero;
        #endregion
        public bool IsConnectted
        {
            get { return isSuccess; }
        }
        public NASStorageProvider(string address, string userName, string userPwd)
        {
            if (!string.IsNullOrEmpty(address))
            {
                isSuccess = Connect(address, userName, userPwd);
            }
            else
            {
                isSuccess = true;
            }
        }

        public byte[] LoadFile(string fileAddress)
        {
            return System.IO.File.ReadAllBytes(fileAddress);
        }

        public bool SaveFile(string fileAddress, string fileName, byte[] bytes)
        {
            if (!Directory.Exists(fileAddress))
            {
                Directory.CreateDirectory(fileAddress);
            }
            System.IO.File.WriteAllBytes(string.Format("{0}\\{1}", fileAddress, fileName), bytes);
            return true;
        }
        #region 建立连接方法
        public void DisConnect()
        {
            if (isSuccess)
            {
                if (impersonatedUser != null)
                    impersonatedUser.Undo();
                if (token != IntPtr.Zero)
                    CloseHandle(token);
            }
        }
        public void Dispose()
        {
            DisConnect();
        }
        public bool Connect(string remoteAddr, string userName, string password)
        {

            bool isSuccess = LogonUser(userName,
                                              remoteAddr,
                                              password,
                                              LOGON32_LOGON_NEW_CREDENTIALS,
                                              LOGON32_PROVIDER_DEFAULT, ref token);

            newIdentity = new WindowsIdentity(token);
            impersonatedUser = newIdentity.Impersonate();
            return true;
        }
        #endregion 


    }
}
