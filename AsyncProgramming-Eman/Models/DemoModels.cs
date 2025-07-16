using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgrammingDemo.Models
{
    /// <summary>
    /// Represents a simulated data service for demo purposes
    /// </summary>
    public class DataService
    {
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Simulates fetching data synchronously
        /// </summary>
        public List<string> GetDataSync(int count)
        {
            List<string> data = new List<string>();
            
            for (int i = 1; i <= count; i++)
            {
                // Simulate processing time
                Thread.Sleep(200);
                data.Add($"Item {i} - {Guid.NewGuid().ToString().Substring(0, 8)}");
            }
            
            return data;
        }
        
        /// <summary>
        /// Simulates fetching data asynchronously
        /// </summary>
        public async Task<List<string>> GetDataAsync(int count)
        {
            List<string> data = new List<string>();
            
            for (int i = 1; i <= count; i++)
            {
                // Simulate processing time
                await Task.Delay(200);
                data.Add($"Item {i} - {Guid.NewGuid().ToString().Substring(0, 8)}");
            }
            
            return data;
        }
        
        /// <summary>
        /// Simulates fetching data asynchronously with progress reporting
        /// </summary>
        public async Task<List<string>> GetDataWithProgressAsync(int count, IProgress<int> progress)
        {
            List<string> data = new List<string>();
            
            for (int i = 1; i <= count; i++)
            {
                // Simulate processing time
                await Task.Delay(200);
                data.Add($"Item {i} - {Guid.NewGuid().ToString().Substring(0, 8)}");
                
                // Report progress
                progress?.Report((i * 100) / count);
            }
            
            return data;
        }
        
        /// <summary>
        /// Simulates fetching data asynchronously with cancellation support
        /// </summary>
        public async Task<List<string>> GetDataWithCancellationAsync(int count, CancellationToken cancellationToken)
        {
            List<string> data = new List<string>();
            
            for (int i = 1; i <= count; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                // Simulate processing time
                await Task.Delay(200, cancellationToken);
                data.Add($"Item {i} - {Guid.NewGuid().ToString().Substring(0, 8)}");
            }
            
            return data;
        }
        
        /// <summary>
        /// Simulates a long-running operation
        /// </summary>
        public Task<int> LongRunningOperationAsync(int iterations)
        {
            return Task.Run(() =>
            {
                int result = 0;
                
                for (int i = 0; i < iterations; i++)
                {
                    // Simulate intensive computation
                    Thread.Sleep(100);
                    result += _random.Next(1, 100);
                }
                
                return result;
            });
        }
        
        /// <summary>
        /// Simulates an operation that might fail
        /// </summary>
        public async Task<string> FallibleOperationAsync(bool shouldFail)
        {
            await Task.Delay(1000);
            
            if (shouldFail)
            {
                throw new InvalidOperationException("Operation failed as requested");
            }
            
            return "Operation completed successfully";
        }
    }
    
    /// <summary>
    /// Represents a simulated file service for demo purposes
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Simulates reading a file synchronously
        /// </summary>
        public string ReadFileSync(string fileName)
        {
            // Simulate file I/O delay
            Thread.Sleep(2000);
            return $"Content of {fileName}: {Guid.NewGuid()}";
        }
        
        /// <summary>
        /// Simulates reading a file asynchronously
        /// </summary>
        public async Task<string> ReadFileAsync(string fileName)
        {
            // Simulate file I/O delay
            await Task.Delay(2000);
            return $"Content of {fileName}: {Guid.NewGuid()}";
        }
        
        /// <summary>
        /// Simulates writing to a file synchronously
        /// </summary>
        public void WriteFileSync(string fileName, string content)
        {
            // Simulate file I/O delay
            Thread.Sleep(1500);
        }
        
        /// <summary>
        /// Simulates writing to a file asynchronously
        /// </summary>
        public async Task WriteFileAsync(string fileName, string content)
        {
            // Simulate file I/O delay
            await Task.Delay(1500);
        }
    }
    
    /// <summary>
    /// Represents a simulated network service for demo purposes
    /// </summary>
    public class NetworkService
    {
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Simulates downloading data synchronously
        /// </summary>
        public byte[] DownloadSync(string url)
        {
            // Simulate network delay
            Thread.Sleep(3000);
            
            // Create random byte array
            byte[] data = new byte[1024];
            _random.NextBytes(data);
            
            return data;
        }
        
        /// <summary>
        /// Simulates downloading data asynchronously
        /// </summary>
        public async Task<byte[]> DownloadAsync(string url)
        {
            // Simulate network delay
            await Task.Delay(3000);
            
            // Create random byte array
            byte[] data = new byte[1024];
            _random.NextBytes(data);
            
            return data;
        }
        
        /// <summary>
        /// Simulates downloading data asynchronously with progress reporting
        /// </summary>
        public async Task<byte[]> DownloadWithProgressAsync(string url, IProgress<int> progress)
        {
            // Create random byte array
            byte[] data = new byte[1024];
            _random.NextBytes(data);
            
            // Simulate download with progress updates
            for (int i = 0; i <= 10; i++)
            {
                await Task.Delay(300);
                progress?.Report(i * 10);
            }
            
            return data;
        }
        
        /// <summary>
        /// Simulates downloading data asynchronously with cancellation support
        /// </summary>
        public async Task<byte[]> DownloadWithCancellationAsync(string url, CancellationToken cancellationToken)
        {
            // Create random byte array
            byte[] data = new byte[1024];
            _random.NextBytes(data);
            
            // Simulate download with cancellation support
            for (int i = 0; i <= 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(300, cancellationToken);
            }
            
            return data;
        }
    }
}
