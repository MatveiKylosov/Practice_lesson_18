using Microsoft.SqlServer.Server;
using PrintManagementSystem_Kylosov.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintManagementSystem_Kylosov.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageWrite.xaml
    /// </summary>
    public partial class PageWrite : Page
    {
        public List<Classes.TypeOpertation> typeOperationList = Classes.TypeOpertation.AllTypeOpertation();
        public List<Classes.Format> formatsList = Classes.Format.AllFormats();

        public PageWrite()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            foreach (Classes.TypeOpertation items in typeOperationList)
                typeOperation.Items.Add(items.name);
            foreach (Classes.Format items in formatsList)
                formats.Items.Add(items.format);
        }
        private void EditOperation(object sender, RoutedEventArgs e)
        {
            if (Operations.SelectedIndex != -1)
            {
                TypeOpertationsWindow newTOW = Operations.Items[Operations.SelectedIndex] as TypeOpertationsWindow;
                typeOperation.SelectedItem = typeOperationList.Find(x => x.id == newTOW.typeOperation).name;
                formats.SelectedItem = formatsList.Find(x => x.id == newTOW.format).format;
                if (newTOW.side == 1) TwoSides.IsChecked = false;
                else if (newTOW.side == 2) TwoSides.IsChecked = true;
                Colors.IsChecked = newTOW.color;
                string[] resultColor = newTOW.colorText.Split('(');
                if (resultColor.Length == 1) LotOfColor.IsChecked = false;
                else if (resultColor.Length == 2) LotOfColor.IsChecked = true;
                textBoxCount.Text = newTOW.count.ToString();
                textBoxPrice.Text = newTOW.price.ToString();
                addOperationButton.Content = "Изменить";
                Operations.Items.Remove(Operations.Items[Operations.SelectedIndex]);
            }
            else MessageBox.Show("Пожалуйста, выберете операцию для редактирования!");

        }
        private void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (Operations.SelectedIndex != -1)
            {
                Operations.Items.Remove(Operations.Items[Operations.SelectedIndex]);
                CalculationsAllPrice();
            }
            else
                MessageBox.Show("Пожалуйста, выберете операцию для удаления.");
        }

        private void SelectedType(object sender, SelectionChangedEventArgs e)
        {
            if (typeOperation.SelectedIndex != -1)
                if (typeOperation.SelectedItem as String == "Сканирование")
                {
                    formats.SelectedIndex = -1;
                    TwoSides.IsChecked = false;
                    Colors.IsChecked = false;
                    LotOfColor.IsChecked = false;
                    formats.IsEnabled = false;
                    TwoSides.IsEnabled = false;
                    Colors.IsEnabled = false;
                    LotOfColor.IsEnabled = false;
                }
                else if (typeOperation.SelectedItem as String == "Печать" || typeOperation.SelectedItem as String == "Копия")
                {
                    formats.IsEnabled = true;
                    TwoSides.IsEnabled = true;
                    Colors.IsEnabled = true;
                    if (formats.SelectedItem as String == "А4")
                    {
                        TwoSides.IsEnabled = true;
                        Colors.IsEnabled = true;
                        LotOfColor.IsEnabled = false;
                    }
                    else if (formats.SelectedItem as String == "А3")
                    {
                        TwoSides.IsEnabled = true;
                        Colors.IsEnabled = false;
                        LotOfColor.IsEnabled = false;
                    }
                    else if (formats.SelectedItem as String == "А2" || formats.SelectedItem as String == "А1")
                    {
                        TwoSides.IsEnabled = false;
                        Colors.IsEnabled = true;
                        LotOfColor.IsEnabled = true;
                    }
                }
                else if (typeOperation.SelectedItem as String == "Ризограф")
                {
                    formats.SelectedIndex = 0;
                    formats.IsEnabled = false;
                    Colors.IsEnabled = false;
                    LotOfColor.IsEnabled = false;
                }
            if (textBoxCount.Text.Length == 0)
                textBoxCount.Text = "1";
            CostCalculations();
        }
        private void SelectedFormat(object sender, SelectionChangedEventArgs e)
        {
            if (formats.SelectedItem as String == "А4")
            {
                TwoSides.IsEnabled = true;
                Colors.IsEnabled = true;
                LotOfColor.IsEnabled = false;
            }
            else if (formats.SelectedItem as String == "А3")
            {
                TwoSides.IsEnabled = true;
                Colors.IsEnabled = false;
                LotOfColor.IsEnabled = false;
            }
            else
            {
                TwoSides.IsEnabled = false;
                Colors.IsEnabled = true;
                LotOfColor.IsEnabled = true;
            }
            if (textBoxCount.Text.Length == 0)
                textBoxCount.Text = "1";
            CostCalculations();
        }

        private void textBoxCount_TextChanged(object sender, TextChangedEventArgs e) => CostCalculations();
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) 
        {
            Regex r = new Regex("[^0-9]+");
            e.Handled = r.IsMatch(e.Text);
        }

        private void AddOperation(object sender, RoutedEventArgs e)
        {
            TypeOpertationsWindow newTOW = new TypeOpertationsWindow();
            newTOW.typeOperationText = typeOperation.SelectedItem as String;
            newTOW.typeOperation = typeOperationsList.Find(x => x.name == newTOW.typeOperationText).id;
            if (formats.SelectedIndex != -1)
            {
                newTOW.formatText = formats.SelectedItem as String;
                newTOW.format = formatsList.Find(x => x.format == newTOW.formatText).id;
            }
            if (TwoSides.IsEnabled == true)
            {
                if (TwoSides.IsChecked == false)
                    newTOW.side = 1;
                else
                    newTOW.side = 2;
            }
            if (Colors.IsChecked == false)
            {
                newTOW.colorText = "Ч/Б";
                newTOW.color = false;
                if (LotOfColor.IsChecked == true)
                {
                    newTOW.colorText += "(> 50%)";
                    newTOW.occupancy = true;
                }
            }
            else
            {
                newTOW.colorText = "ЦВ";
                newTOW.color = true;
                if (LotOfColor.IsChecked == true)
                {
                    newTOW.colorText += "(> 50%)";
                    newTOW.occupancy = true;
                }
            }
            newTOW.count = int.Parse(textBoxCount.Text);
            newTOW.price = float.Parse(textBoxPrice.Text);
            addOperationButton.Content = "Добавить";
            Operations.Items.Add(newTOW);
            CalculationsAllPrice();
        }
        private void ColorsChange(object sender, RoutedEventArgs e) { }

        public void CalculationsAllPrice()
        {
            float allPrice = 0;
            for (int i = 0; i < Operations.Items.Count; i++)
            {
                TypeOpertationsWindow newTOW = Operations.Items[i] as TypeOpertationsWindow;
                allPrice += newTOW.price;
            }
            labelAllPrice.Content = "Общая сумма: " + allPrice;
        }
        public void CostCalculations()
        {
            float price = 0;
            int count = int.Parse(textBoxCount.Text);
            int countText = textBoxCount.Text.Length;

            string typeOp = typeOperation.SelectedItem as String;
            string format = formats.SelectedItem as String;

            bool twoSides = (bool)TwoSides.IsChecked;
            bool colors = (bool)Colors.IsChecked;
            bool lotOfColor = (bool)LotOfColor.IsChecked;

            if (typeOperation.SelectedIndex != -1)
            {
                if (typeOp == "Сканирование")
                    price = 10;
                else if (typeOp == "Печать" || typeOp == "Копия")
                {
                    if (format == "А4")
                    {
                        if (!twoSides)
                            if (!colors)
                                price = (countText > 0 && count < 30) ? 4 : 3;
                            else
                            {
                                if (!colors)
                                    price = (countText > 0 && count < 30) ? 6 : 4;

                                else price = 35;
                            }
                    }
                    else if (format == "А3")
                    {
                        if (!twoSides)
                            if (!colors)
                                price = (countText > 0 && count < 30) ? 8 : 6;
                            else
                            {
                                if (!colors)
                                    price = (countText > 0 && count < 30) ? 12 : 10;
                            }
                    }
                    else if (format == "А2")
                    {
                        if (!colors)
                            price = !lotOfColor ? 35 : 50;
                        else
                            price = !lotOfColor ? 120 : 170;
                    }
                    else if (format == "А1")
                    {
                        if (!colors)
                            price = !lotOfColor ? 75 : 120;
                        else
                            price = !lotOfColor ? 170 : 250;
                    }
                }
                else if (typeOp == "Ризограф")
                {
                    if (!twoSides)
                    {
                        if (countText > 0 && count < 100) price = 1.40f;
                        else if (count < 200 && countText > 0 && count >= 100) price = 1.10f;
                        else price = 1;
                    }
                    else
                    {
                        if (countText > 0 && count < 100)
                            price = 1.80f;
                        else if (countText > 0 && count < 200 && count >= 100)
                            price = 1.40f;
                        else price = 1.10f;
                    }
                }
            }

            if (countText > 0)
                textBoxPrice.Text = (float.Parse(textBoxCount.Text) * price).ToString();
        }

    }
}
