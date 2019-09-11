using System;
using System.IO;
using UnityEngine;
using CsvHelper;

namespace Vhil.DataCollector
{
    public static class Extensions
    {
        public static void Write(this DataWriter<TrackingData> trackingDataWriter, Transform transform)
        {
            trackingDataWriter.Write(TrackingData.Create(transform));
        }
    }

    public class TrackingData
    {
        public float TimeStamp { get; private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }
        public float RotationW { get; private set; }
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }

        public TrackingData(float timeStamp, float positionX, float positionY, float positionZ,
                            float rotationW, float rotationX, float rotationY, float rotationZ)
        {
            TimeStamp = timeStamp;
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
            RotationW = rotationW;
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
        }

        public static TrackingData Create(float time, Transform transform)
        {
            return new TrackingData(timeStamp: time,
                                    positionX: transform.localPosition.x,
                                    positionY: transform.localPosition.y,
                                    positionZ: transform.localPosition.z,
                                    rotationW: transform.localRotation.w,
                                    rotationX: transform.localRotation.x,
                                    rotationY: transform.localRotation.y,
                                    rotationZ: transform.localRotation.z);
        }

        public static TrackingData Create(Transform transform)
        {
            return Create(Time.time, transform);
        }
    }
    public class DataWriter<T> : IDisposable
    {
        private readonly CsvWriter writer;

        private DataWriter(CsvWriter writer)
        {
            this.writer = writer;
        }

        public static DataWriter<T> Create(string path)
        {
            var streamWriter = new StreamWriter(path);
            var csvWriter = new CsvHelper.CsvWriter(streamWriter);
            csvWriter.WriteHeader<T>();
            csvWriter.NextRecord();
            return new DataWriter<T>(csvWriter);
        }

        public void Write(T t)
        {
            writer.WriteRecord(t);
            writer.NextRecord();
        }

        public void Dispose()
        {
            writer.Dispose();
        }
    }
}