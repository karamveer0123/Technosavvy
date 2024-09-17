using Microsoft.EntityFrameworkCore;

namespace NavExM.Int.Maintenance.APIs.Services
{
    internal class SrvNegotiateWallets : AppConfigBase
    {
        protected override async Task DoStart()
        {
            //Notify WatchDog and Close, if not overridden in Drived class

            /* This Class Should hold all the task that it is suppose to do 
             * This Class will remain in context for the duration of the App Life, So as good as Static
             * Read RabbitMQ
             * Update Local Collection
             * Get & Update External Network Address
             * Check and Update Staking renewal
             */
            Console.WriteLine($"NegotiateWallets is waiting..{DateTime.UtcNow}-{Environment.CurrentManagedThreadId}");
            await Task.Delay(500);
            Console.WriteLine($"NegotiateWallets waiting complete..{DateTime.UtcNow}-P{Environment.ProcessId}-T{Environment.CurrentManagedThreadId}");

            await Task.CompletedTask;
        }
        private void EnsureDefaultWallets()
        {
            /*  1. NavC Pool Fund
             *  2. Operation Wallet for All The Change Swap Fee
             *  3. Employee Wallets for Aritrage or Other Actions
             * 
             */
        }
    }
}
