using System.Text.RegularExpressions;
using HubtelWalletDatabase.DataAccess;
using HubtelWalletModel;

namespace HubtelWalletAPI.BussinesLogic
{
    public static class ValidateWallet
    {
        const string s_momoType = "momo";
        const string s_cardType = "card";
        static readonly string[] s_momoSchema = { "mtn", "vodafone", "airteltigo" };
        static readonly string[] s_cardSchema = { "visa", "mastercard" };

        public static async Task<string> ValidateWalletAsync(Wallet wallet, WalletDataAccess dataAccess, string user)
        {
            if (wallet is null)
                return "Invalid wallet";

            var existingWallet = await dataAccess.GetWallet(wallet.AccountNumber);

            if (existingWallet != null)
                return "This wallet already exists.";

            // a single user should NOT have more than 5 wallets

            if ((await dataAccess.GetWalletCount(user)) >= 5)
                return "You cannot have more than 5 wallets.";

            // validate account number and scheme based on type
            if (wallet.Type == s_momoType)
            {
                if (IsPhoneNumber(wallet.AccountNumber))
                   return "The account number must be a valid phone number.";

                if (!IsValidMomoSchema(wallet.AccountScheme))
                    return "The account scheme must be either mtn, vodafone or airteltigo.";
            }
            else if (wallet.Type == s_cardType)
            {
                if (!IsValidCardSchema(wallet.AccountScheme))
                {
                    return "The account scheme must be either visa or mastercard.";
                }
            }
            else
            {
                return "The type must be either momo or card.";
            }

            // only first 6 digits of card number should be stored
            if (wallet.Type.Equals(s_cardType, StringComparison.CurrentCultureIgnoreCase))
            {
                wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
            }

            return string.Empty;
        }

        private static bool IsPhoneNumber(string number)
        {
            var phoneRegex = new Regex(@"^\+233\d{9}$");
            return phoneRegex.IsMatch(number);
        }
        private static bool IsValidCardSchema(object accountScheme)
        {
            return s_cardSchema.Contains(accountScheme);
        }

        private static bool IsValidMomoSchema(string schema)
        {
            return s_momoType.Contains(schema);
        }

        private static bool IsCardNumber(string s)
        {
            var regex = @"^\d{16}$";
            return Regex.IsMatch(s, regex);
        }
    }
}
