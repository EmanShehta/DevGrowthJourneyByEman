using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingConsoleDemo.Models
{
    /// <summary>
    /// Represents a simple product for demonstration purposes
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"Product {Id}: {Name} - ${Price:F2} (Stock: {Stock})";
        }
    }

    /// <summary>
    /// Represents a simple order for demonstration purposes
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }

        public override string ToString()
        {
            return $"Order {Id}: {CustomerName} - ${TotalAmount:F2} - Status: {Status}";
        }
    }

    /// <summary>
    /// Represents an item in an order
    /// </summary>
    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{ProductName} x {Quantity} @ ${Price:F2} each";
        }
    }

    /// <summary>
    /// Represents the status of an order
    /// </summary>
    public enum OrderStatus
    {
        New,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    /// <summary>
    /// Represents a bank account for thread safety demonstrations
    /// </summary>
    public class BankAccount
    {
        private decimal _balance;
        private readonly object _lockObject = new object();
        public string AccountNumber { get; }
        public string OwnerName { get; }

        public BankAccount(string accountNumber, string ownerName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            _balance = initialBalance;
        }

        public decimal Balance
        {
            get
            {
                lock (_lockObject)
                {
                    return _balance;
                }
            }
        }

        // Unsafe deposit method (no locking)
        public void DepositUnsafe(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive", nameof(amount));
            }

            // Simulate some processing time
            Thread.Sleep(10);

            // This operation is not atomic and can cause race conditions
            decimal newBalance = _balance + amount;
            
            // Simulate more processing time
            Thread.Sleep(10);
            
            _balance = newBalance;
        }

        // Safe deposit method (with locking)
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive", nameof(amount));
            }

            lock (_lockObject)
            {
                // Simulate some processing time
                Thread.Sleep(10);

                // This operation is atomic because of the lock
                _balance += amount;
            }
        }

        // Unsafe withdrawal method (no locking)
        public bool WithdrawUnsafe(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
            }

            // Simulate some processing time
            Thread.Sleep(10);

            // Check if there are sufficient funds
            if (_balance < amount)
            {
                return false;
            }

            // Simulate more processing time
            Thread.Sleep(10);

            // This operation is not atomic and can cause race conditions
            _balance -= amount;
            return true;
        }

        // Safe withdrawal method (with locking)
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
            }

            lock (_lockObject)
            {
                // Check if there are sufficient funds
                if (_balance < amount)
                {
                    return false;
                }

                // Simulate some processing time
                Thread.Sleep(10);

                // This operation is atomic because of the lock
                _balance -= amount;
                return true;
            }
        }

        // Transfer method that can cause deadlocks if not careful
        public bool TransferTo(BankAccount destination, decimal amount)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be positive", nameof(amount));
            }
            lock (_lockObject)
            {
                // Check if there are sufficient funds
                if (_balance < amount)
                {
                    return false;
                }
                // Simulate some processing time
                Thread.Sleep(100);
                lock (destination._lockObject)
                {
                    // Withdraw from this account
                    _balance -= amount;
                    // Deposit to destination account
                    destination._balance += amount;
                    return true;
                }
            }
        }

        // Safe transfer method that prevents deadlocks by acquiring locks in a consistent order
        public bool SafeTransferTo(BankAccount destination, decimal amount)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be positive", nameof(amount));
            }

            // Determine which lock to acquire first based on account number to prevent deadlocks
            bool acquireThisFirst = string.Compare(AccountNumber, destination.AccountNumber, StringComparison.Ordinal) < 0;
            
            object firstLock = acquireThisFirst ? _lockObject : destination._lockObject;
            object secondLock = acquireThisFirst ? destination._lockObject : _lockObject;

            lock (firstLock)
            {
                // Simulate some processing time
                Thread.Sleep(100);

                lock (secondLock)
                {
                    // Check if there are sufficient funds
                    if (_balance < amount)
                    {
                        return false;
                    }

                    // Withdraw from this account
                    _balance -= amount;

                    // Deposit to destination account
                    destination._balance += amount;

                    return true;
                }
            }
        }

        public override string ToString()
        {
            return $"Account {AccountNumber}: {OwnerName} - Balance: ${Balance:F2}";
        }
    }
}
