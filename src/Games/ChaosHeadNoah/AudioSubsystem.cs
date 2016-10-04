using NVorbis;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChaosHeadNoah
{
    public sealed class AudioSubsystem : IDisposable
    {
        private readonly XAudio2 _device;
        private readonly MasteringVoice _masteringVoice;

        public AudioSubsystem()
        {
            _device = new XAudio2();
            _masteringVoice = new MasteringVoice(_device);
        }

        public void PlaySound(Stream stream)
        {
            Task.Run(async () => await PlaySoundImpl(stream));
        }

        private async Task PlaySoundImpl(Stream stream)
        {
            using (var vorbis = new VorbisReader(stream, true))
            {
                var channels = vorbis.Channels;
                var sampleRate = vorbis.SampleRate;
                var samples = new float[channels * sampleRate];

                var format = new WaveFormat(sampleRate, 32, 2);
                var src = new SourceVoice(_device, format);

                var bufferQueue = new Queue<AudioBuffer>();
                src.BufferEnd += (IntPtr _) =>
                {
                    bufferQueue.Dequeue().Stream.Dispose();
                };

                src.Start();

                bool doneReading = false;
                do
                {
                    if (src.State.BuffersQueued < 3 && !doneReading)
                    {
                        int bytesRead = vorbis.ReadSamples(samples, 0, samples.Length);
                        if (bytesRead == 0)
                        {
                            doneReading = true;
                            continue;
                        }

                        var dataStream = new DataStream(bytesRead * sizeof(float), true, true);
                        dataStream.WriteRange(samples, 0, bytesRead);
                        dataStream.Position = 0;

                        var buffer = new AudioBuffer(dataStream);
                        buffer.Flags = BufferFlags.EndOfStream;
                        bufferQueue.Enqueue(buffer);
                        src.SubmitSourceBuffer(buffer, null);
                    }

                    await Task.Delay(100).ConfigureAwait(false);
                } while (src.State.BuffersQueued > 0);

                src.DestroyVoice();
                src.Dispose();
            }
        }

        public void Dispose()
        {
            _masteringVoice?.Dispose();
            _device?.Dispose();
        }
    }
}
