using PracticeWork10M4.Models;
using PracticeWork10M4.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWork10M4.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly RealEstateRestService _restService = new();

    public MainViewModel()
    {
        _ = LoadAsync();
    }

    public ObservableCollection<RealEstateObject> RealEstateObjects { get; } = new();

    public List<string> Statuses { get; } = new()
    {
        "Вільний",
        "Орендований",
        "На ремонті",
        "Заброньований"
    };

    private RealEstateObject? _selectedRealEstateObject;

    public RealEstateObject? SelectedRealEstateObject
    {
        get => _selectedRealEstateObject;
        set
        {
            if (SetProperty(ref _selectedRealEstateObject, value))
            {
                FillFormFromSelectedObject();
            }
        }
    }

    private string _formTitle = string.Empty;

    public string FormTitle
    {
        get => _formTitle;
        set => SetProperty(ref _formTitle, value);
    }

    private string _formAddress = string.Empty;

    public string FormAddress
    {
        get => _formAddress;
        set => SetProperty(ref _formAddress, value);
    }

    private string _formOwnerName = string.Empty;

    public string FormOwnerName
    {
        get => _formOwnerName;
        set => SetProperty(ref _formOwnerName, value);
    }

    private double _formPrice;

    public double FormPrice
    {
        get => _formPrice;
        set => SetProperty(ref _formPrice, value);
    }

    private string _formRentalStatus = "Вільний";

    public string FormRentalStatus
    {
        get => _formRentalStatus;
        set => SetProperty(ref _formRentalStatus, value);
    }

    private string _errorMessage = string.Empty;

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            List<RealEstateObject> items = await _restService.GetObjectsAsync();

            RealEstateObjects.Clear();

            foreach (RealEstateObject item in items.OrderBy(x => x.Id))
            {
                RealEstateObjects.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка завантаження даних: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task SaveObjectAsync()
    {
        try
        {
            ErrorMessage = string.Empty;

            RealEstateObject realEstateObject = new()
            {
                Id = SelectedRealEstateObject?.Id ?? 0,
                Title = FormTitle,
                Address = FormAddress,

                Type = SelectedRealEstateObject?.Type ?? 0,
                Area = SelectedRealEstateObject?.Area ?? 0,
                Rooms = SelectedRealEstateObject?.Rooms ?? 1,

                Price = (decimal)FormPrice,
                SalePrice = SelectedRealEstateObject?.SalePrice ?? 0,

                RentalStatus = ConvertStatusToInt(FormRentalStatus),

                OwnerName = FormOwnerName,
                OwnerPhone = SelectedRealEstateObject?.OwnerPhone ?? string.Empty,

                CreatedAt = SelectedRealEstateObject?.CreatedAt ?? DateTime.Now
            };

            if (SelectedRealEstateObject is null)
            {
                await _restService.CreateObjectAsync(realEstateObject);
            }
            else
            {
                await _restService.UpdateObjectAsync(realEstateObject);
            }

            await LoadAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка збереження: {ex.Message}";
        }
    }

    public async Task DeleteSelectedObjectAsync()
    {
        if (SelectedRealEstateObject is null)
        {
            ErrorMessage = "Оберіть об'єкт для видалення.";
            return;
        }

        try
        {
            ErrorMessage = string.Empty;

            await _restService.DeleteObjectAsync(SelectedRealEstateObject.Id);

            await LoadAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка видалення: {ex.Message}";
        }
    }

    public void ClearForm()
    {
        SelectedRealEstateObject = null;

        FormTitle = string.Empty;
        FormAddress = string.Empty;
        FormOwnerName = string.Empty;
        FormPrice = 0;
        FormRentalStatus = "Вільний";
    }

    private void FillFormFromSelectedObject()
    {
        if (SelectedRealEstateObject is null)
            return;

        FormTitle = SelectedRealEstateObject.Title;
        FormAddress = SelectedRealEstateObject.Address;
        FormOwnerName = SelectedRealEstateObject.OwnerName;
        FormPrice = (double)SelectedRealEstateObject.Price;
        FormRentalStatus = ConvertStatusToText(SelectedRealEstateObject.RentalStatus);
    }
    private int ConvertStatusToInt(string status)
    {
        return status switch
        {
            "Вільний" => 0,
            "Орендований" => 1,
            "На ремонті" => 2,
            "Заброньований" => 3,
            _ => 0
        };
    }

    private string ConvertStatusToText(int status)
    {
        return status switch
        {
            0 => "Вільний",
            1 => "Орендований",
            2 => "На ремонті",
            3 => "Заброньований",
            _ => "Вільний"
        };
    }
}