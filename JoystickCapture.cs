using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
/// GamePad Utility.
/// @author sanyo[JP]
/// @version 1.0
class JoystickCapture
{
  const int JOY_RETURNALL = 0x0FF;

  [DllImport("winmm.dll")]
  private static extern int joyGetPosEx(int uJoyID, ref JOYINFOEX pji);

  [StructLayout(LayoutKind.Sequential)]
  private struct JOYINFOEX {
    public int dwSize;
    public int dwFlags;
    public int dwXpos;
    public int dwYpos;
    public int dwZpos;
    public int dwRpos;
    public int dwUpos;
    public int dwVpos;
    public int dwButtons;
    public int dwButtonNumber;
    public int dwPOV;
    public int dwReserved1;
    public int dwReserved2;
  }

  [STAThread]
  static void Main(string[] args)
  {
    int milliseconds = 8;
    if(args.Length > 0){
      int.TryParse(args[0], out milliseconds);
      milliseconds = Math.Max(Math.Min(milliseconds, 1000),5);
    }

    int joystickId = 0;
    if(args.Length > 1){
      int.TryParse(args[1], out joystickId);
    }

    while(true){
      var joyInfo = new JOYINFOEX();
      joyInfo.dwSize = Marshal.SizeOf(joyInfo);
      joyInfo.dwFlags = JOY_RETURNALL;
      int mmresultCode = joyGetPosEx(joystickId, ref joyInfo);
      if(mmresultCode != 0)
        break;
      Console.WriteLine("{0:0.0000},{1:0.0000},{2:0.0000},{3:0.0000},{4:0.0000},{5:0.0000}",joyInfo.dwXpos/(float)0xffff,joyInfo.dwYpos/(float)0xffff,joyInfo.dwZpos/(float)0xffff,joyInfo.dwRpos/(float)0xffff,joyInfo.dwUpos/(float)0xffff,joyInfo.dwVpos/(float)0xffff);
      Thread.Sleep(milliseconds);
    }
  }
}