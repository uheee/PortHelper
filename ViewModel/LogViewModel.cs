using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PortHelper.ViewModel
{
    public sealed class LogViewModel : INotifyPropertyChanged
    {
        #region Fields

        private byte[] _content;

        private bool _isTextMode;
        private string _source;

        private string _text;

        #endregion Fields

        public LogViewModel()
        {
            Time = DateTime.Now;
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public byte[] Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public bool IsSystemLog { get; set; }

        public bool IsTextMode
        {
            get => _isTextMode;
            set
            {
                _isTextMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Text));
            }
        }

        public string Source
        {
            get => _source;
            set
            {
                if (_source == value) return;
                _source = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get
            {
                if (_isTextMode || IsSystemLog) return Encoding.UTF8.GetString(Content);

                var hexString = BitConverter.ToString(Content);
                return hexString.Replace('-', ' ');
            }
            set
            {
                _text = value;
                if (_isTextMode || IsSystemLog)
                {
                    Content = Encoding.UTF8.GetBytes(value);
                }
                else
                {
                    _text = _text.Replace(" ", "");
                    Content = new byte[_text.Length / 2];
                    for (var i = 0; i < _content.Length; i++)
                        _content[i] = Convert.ToByte(_text.Substring(i * 2, 2), 16);

                    _text = Encoding.UTF8.GetString(Content);
                }

                OnPropertyChanged();
            }
        }

        public DateTime Time { get; }

        #endregion Properties

        #region Methods

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}