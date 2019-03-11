using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCV.Net;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Diagnostics;


namespace Bonsai.FileCaptureRAW
{
    public class FileCaptureRAW : Source<IplImage>
    {

        public override IObservable<IplImage> Generate()
        {
            return source;
        }

        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("The name of the movie file.")]
        public string FileName { get; set; }

        public double FrameRate { get; set; }

        public bool Replay { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }

        private IObservable<IplImage> source;

        public FileCaptureRAW()
        {
            source = Observable.Create<IplImage>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    string fileName = FileName;
                    double frameRate = FrameRate;
                    bool replay = Replay;
                    int startPosition = StartPosition;
                    int endPosition = EndPosition;
                    Capture capture = Capture.CreateFileCapture(fileName);
                    int frameCount = (int) capture.GetProperty(CaptureProperty.FrameCount);
                    int i;
                    if (startPosition < 0 || startPosition >= frameCount)
                    {
                        startPosition = 0;
                    }
                    if (endPosition <= startPosition || endPosition > frameCount)
                    {
                        endPosition = frameCount;
                        EndPosition = endPosition;
                    }
                    i = startPosition;
                    StartPosition = i;
                    if (frameRate <= 0)
                    {
                        frameRate = capture.GetProperty(CaptureProperty.Fps);
                        FrameRate = frameRate;
                    }
                    IplImage frame;
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        capture.SetProperty(CaptureProperty.PosFrames, i);
                        if (i < endPosition - 1)
                        {
                            i++;
                        }
                        else
                        {
                            if (replay)
                            {
                                i = startPosition;
                            }                       
                        }
                        frame = capture.QueryFrame().Clone();
                        observer.OnNext(frame);
                        Thread.Sleep((int) (1000 / frameRate));
                    }
                    observer.OnCompleted();
                    capture.Dispose();
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            })
            .PublishReconnectable()
            .RefCount();
        }
    }
}