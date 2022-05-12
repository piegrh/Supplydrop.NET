using Newtonsoft.Json;

namespace Webhallen.Models
{
    public partial class MeResponse
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("starredStores")]
        public object[] StarredStores { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("isCorporateCustomer")]
        public bool IsCorporateCustomer { get; set; }

        [JsonProperty("experiencePoints")]
        public long ExperiencePoints { get; set; }

        [JsonProperty("corporateNumber")]
        public string CorporateNumber { get; set; }

        [JsonProperty("corporateName")]
        public string CorporateName { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("invoiceEmail")]
        public string InvoiceEmail { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("rankLevel")]
        public long RankLevel { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("ssn")]
        public string Ssn { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("isEmployee")]
        public bool IsEmployee { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("feed")]
        public object[] Feed { get; set; }

        [JsonProperty("storeSuggestions")]
        public object[] StoreSuggestions { get; set; }

        [JsonProperty("storeWeights")]
        public object[] StoreWeights { get; set; }

        [JsonProperty("paymentSuggestions")]
        public long[] PaymentSuggestions { get; set; }

        [JsonProperty("paymentWeights")]
        public long[] PaymentWeights { get; set; }

        [JsonProperty("shippingSuggestions")]
        public long[] ShippingSuggestions { get; set; }

        [JsonProperty("shippingWeights")]
        public long[] ShippingWeights { get; set; }

        [JsonProperty("showTitleTwo")]
        public bool ShowTitleTwo { get; set; }
    }

    public partial class Avatar
    {
        [JsonProperty("class")]
        public Class Class { get; set; }
    }

    public partial class Class
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class Settings
    {
        [JsonProperty("product_list_mode")]
        public long ProductListMode { get; set; }

        [JsonProperty("no_focus_search")]
        public bool NoFocusSearch { get; set; }

        [JsonProperty("send_reminder_mail")]
        public bool SendReminderMail { get; set; }

        [JsonProperty("show_prices_excluding_vat")]
        public bool ShowPricesExcludingVat { get; set; }

        [JsonProperty("send_email_receipt")]
        public bool SendEmailReceipt { get; set; }

        [JsonProperty("show_member_tutorial")]
        public bool ShowMemberTutorial { get; set; }

        [JsonProperty("print_waybill")]
        public bool PrintWaybill { get; set; }

        [JsonProperty("auto_open_admin_panel")]
        public bool AutoOpenAdminPanel { get; set; }

        [JsonProperty("default_to_admin_search")]
        public bool DefaultToAdminSearch { get; set; }

        [JsonProperty("open_products_in_admin")]
        public bool OpenProductsInAdmin { get; set; }

        [JsonProperty("show_member_prices_as_admin")]
        public bool ShowMemberPricesAsAdmin { get; set; }

        [JsonProperty("open_classic_admin_in_new_tab")]
        public bool OpenClassicAdminInNewTab { get; set; }

        [JsonProperty("serious_business_mode")]
        public bool SeriousBusinessMode { get; set; }

        [JsonProperty("send_newsletter")]
        public bool SendNewsletter { get; set; }

        [JsonProperty("member_after_checkout")]
        public bool MemberAfterCheckout { get; set; }

        [JsonProperty("newsletter_option_modified_at")]
        public long NewsletterOptionModifiedAt { get; set; }

        [JsonProperty("show_newsletter_popup")]
        public bool ShowNewsletterPopup { get; set; }

        [JsonProperty("send_sms")]
        public bool SendSms { get; set; }

        [JsonProperty("membership_cancelled_at")]
        public long MembershipCancelledAt { get; set; }

        [JsonProperty("member_level_threshold_at")]
        public long MemberLevelThresholdAt { get; set; }

        [JsonProperty("member_gained_xp_at")]
        public long MemberGainedXpAt { get; set; }

        [JsonProperty("member_level_changed_at")]
        public long MemberLevelChangedAt { get; set; }

        [JsonProperty("member_class_changed_at")]
        public long MemberClassChangedAt { get; set; }

        [JsonProperty("voyado_email_group")]
        public long VoyadoEmailGroup { get; set; }

        [JsonProperty("voyado_email_group_active")]
        public long VoyadoEmailGroupActive { get; set; }

        [JsonProperty("voyado_sms_option_modified_at")]
        public long VoyadoSmsOptionModifiedAt { get; set; }

        [JsonProperty("voyado_email_option_modified_at")]
        public long VoyadoEmailOptionModifiedAt { get; set; }

        [JsonProperty("test_user")]
        public long TestUser { get; set; }
    }
}
