# Bets Brasileiras

🇧🇷 🎰 📋 Brazilian (authorized) bets list

**Bets Brasileiras** is a repository that maintains a list of authorized betting companies in Brazil, as approved by the government. This project provides various formats (XML, CSV, JSON, and SQL) of the list for easy access and integration into different systems.

## 📝 Data Schema

Each entry contains the following data:

- **Number and Year of Requirement**: Identification of the regulatory request.
- **Company Name**: Official name as per the company registration with the Brazilian Federal Revenue (RFB).
- **Company Number (CNPJ)**: Official business registration number.
- **Brands/Sites Names**: Multiple brands can belong to the same company.
- **Domain**: The main domain for each brand, with the possibility of multiple domains per company.

## 🔄 Regular Updates

The list is updated regularly via an automated tool that fetches the latest government-approved list and updates the repository.

## 💻 Formats Available

- **XML**: Structured data for easy integration with XML-based systems.
- **CSV**: Comma-separated values for easy import into spreadsheets or databases.
- **JSON**: Lightweight data interchange format for use in web applications.
- **SQL**: Structured query language entries for direct database use.

## ⚙️ How to Use

1. Clone the repository:

   ```bash
   git clone https://github.com/guibranco/BetsBrasileiras.git
   ```

2. Navigate to the folder containing the data files.

3. Choose the format you need (XML, CSV, JSON, or SQL) and integrate it with your application or system.

4. Run the update tool to fetch the latest list:

   ```bash
   dotnet run updateData
   ```

   This will automatically update the files in the repository.

## 📈 Example Data

Here’s an example of what the data looks like:

### JSON Format:

```json
{
  "requirement_number_year": "12345/2025",
  "fiscal_name": "BetCompany XYZ",
  "document": "12.345.678/0001-90",
  "brand": "BrandOne",
  "domain": "www.brandone.com"
}
```

### CSV Format:

```csv
Requirement Number/Year, Fiscal Name, Document, Brand, Domain
12345/2025, BetCompany XYZ, 12.345.678/0001-90, BrandOne, www.brandone.com
12345/2025, BetCompany XYZ, 12.345.678/0001-90, BrandTwo, www.brandtwo.com
12345/2025, BetCompany XYZ, 12.345.678/0001-90, BrandTwo, www.brandtwo-br.com
```

## 🤝 Contributing

If you would like to contribute to this project:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit (`git commit -am 'Add new bet company'`).
4. Push to the branch (`git push origin feature-branch`).
5. Open a Pull Request.

## 📅 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🌍 Links

- [Official Government List of Authorized Bets](https://www.gov.br/fazenda/pt-br/composicao/orgaos/secretaria-de-premios-e-apostas/lista-de-empresas)
- [Bets Brasileiras Documentation](http://guilherme.stracini.com.br/BetsBrasileiras/)
