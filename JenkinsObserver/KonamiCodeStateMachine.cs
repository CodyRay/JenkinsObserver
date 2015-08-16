using System.Windows.Input;

namespace JenkinsObserver
{
    public delegate void KonamiCodeEvent(object sender);

    //↑↑↓↓←→←→BA
    public class KonamiCodeStateMachine
    {
        public event KonamiCodeEvent KonamiCodeEntered;

        private int KonamiCodeCount { get; set; }

        public void KeyPressed(Key keyPressed)
        {
            //*123456789
            //↑↑↓↓←→←→BA
            //1234567890
            switch (keyPressed)
            {
                case Key.Up:
                    if (KonamiCodeCount == 1)
                        KonamiCodeCount++;
                    else
                        KonamiCodeCount = 1;
                    break;

                case Key.Down:
                    if (KonamiCodeCount == 2 || KonamiCodeCount == 3)
                        KonamiCodeCount++;
                    break;

                case Key.Left:
                    if (KonamiCodeCount == 4 || KonamiCodeCount == 6)
                        KonamiCodeCount++;
                    break;

                case Key.Right:
                    if (KonamiCodeCount == 5 || KonamiCodeCount == 7)
                        KonamiCodeCount++;
                    break;

                case Key.B:
                    if (KonamiCodeCount == 8)
                        KonamiCodeCount++;
                    break;

                case Key.A:
                    if (KonamiCodeCount == 9)
                        KonamiCodeCount++;
                    break;

                default:
                    KonamiCodeCount = 0;
                    break;
            }

            if (KonamiCodeCount == 10)
                KonamiCodeEntered?.Invoke(this);
        }
    }
}