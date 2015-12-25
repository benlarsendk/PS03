using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace PS03.PasswordOps.FirefoxDecrypt
{
    internal class FFDecryptOps
    {
        private IntPtr _nssModule;

        private IntPtr LoadWin32Library(string libPath)
        {
            if (string.IsNullOrEmpty(libPath))
                throw new ArgumentNullException("libPath");

            var moduleHandle = LoadLibrary(libPath);
            if (moduleHandle == IntPtr.Zero)
            {
                var lasterror = Marshal.GetLastWin32Error();
                var innerEx = new Win32Exception(lasterror);
                innerEx.Data.Add("LastWin32Error", lasterror);

                throw new Exception("can't load DLL " + libPath, innerEx);
            }
            return moduleHandle;
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        public void Init(string profile)
        {
            var user = Environment.UserName;
            LoadLibrary("C:/Program Files (x86)/Mozilla Firefox/msvcp120.dll");
            LoadLibrary("C:/Program Files (x86)/Mozilla Firefox/msvcr120.dll");
            LoadLibrary("C:/Program Files (x86)/Mozilla Firefox/mozglue.dll");
            _nssModule = LoadLibrary("C:/Program Files (x86)/Mozilla Firefox/nss3.dll");
            var pProc = GetProcAddress(_nssModule, "NSS_Init");
            var NSS_Init = (NSS_InitPtr) Marshal.GetDelegateForFunctionPointer(pProc, typeof (NSS_InitPtr));
            NSS_Init(profile);
            var keySlot = PK11_GetInternalKeySlot();
            PK11_Authenticate(keySlot, true, 0);
        }

        private long PK11_GetInternalKeySlot()
        {
            var pProc = GetProcAddress(_nssModule, "PK11_GetInternalKeySlot");
            var ptr =
                (PK11_GetInternalKeySlotPtr)
                    Marshal.GetDelegateForFunctionPointer(pProc, typeof (PK11_GetInternalKeySlotPtr));
            return ptr();
        }

        private long PK11_Authenticate(long slot, bool loadCerts, long wincx)
        {
            var pProc = GetProcAddress(_nssModule, "PK11_Authenticate");
            var ptr = (PK11_AuthenticatePtr) Marshal.GetDelegateForFunctionPointer(pProc, typeof (PK11_AuthenticatePtr));
            return ptr(slot, loadCerts, wincx);
        }

        private int NSSBase64_DecodeBuffer(IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen)
        {
            var pProc = GetProcAddress(_nssModule, "NSSBase64_DecodeBuffer");
            var ptr =
                (NSSBase64_DecodeBufferPtr)
                    Marshal.GetDelegateForFunctionPointer(pProc, typeof (NSSBase64_DecodeBufferPtr));
            return ptr(arenaOpt, outItemOpt, inStr, inLen);
        }

        private int PK11SDR_Decrypt(ref TSECItem data, ref TSECItem result, int cx)
        {
            var pProc = GetProcAddress(_nssModule, "PK11SDR_Decrypt");
            var ptr = (PK11SDR_DecryptPtr) Marshal.GetDelegateForFunctionPointer(pProc, typeof (PK11SDR_DecryptPtr));
            return ptr(ref data, ref result, cx);
        }

        public string Decrypt(string cypherText)
        {
            var sb = new StringBuilder(cypherText);
            var hi2 = NSSBase64_DecodeBuffer(IntPtr.Zero, IntPtr.Zero, sb, sb.Length);
            var tSecDec = new TSECItem();
            var item = (TSECItem) Marshal.PtrToStructure(new IntPtr(hi2), typeof (TSECItem));
            if (PK11SDR_Decrypt(ref item, ref tSecDec, 0) == 0)
            {
                if (tSecDec.SECItemLen != 0)
                {
                    var bvRet = new byte[tSecDec.SECItemLen];
                    Marshal.Copy(new IntPtr(tSecDec.SECItemData), bvRet, 0, tSecDec.SECItemLen);
                    return Encoding.UTF8.GetString(bvRet);
                }
            }
            return null;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long NSS_InitPtr(string configdir);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PK11SDR_DecryptPtr(ref TSECItem data, ref TSECItem result, int cx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_GetInternalKeySlotPtr();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long PK11_AuthenticatePtr(long slot, bool loadCerts, long wincx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int NSSBase64_DecodeBufferPtr(
            IntPtr arenaOpt, IntPtr outItemOpt, StringBuilder inStr, int inLen);

        [StructLayout(LayoutKind.Sequential)]
        private struct TSECItem
        {
            public readonly int SECItemType;
            public readonly int SECItemData;
            public readonly int SECItemLen;
        }
    }
}