<?php

// This is a autogenerated file:Bet

class Bet {
    private string $brand; // json:brand Required
    private string $document; // json:document Required
    private string $domain; // json:domain Required
    private string $fiscalName; // json:fiscal_name Required
    private string $requirementNumberYear; // json:requirement_number_year Required

    /**
     * @param string $brand
     * @param string $document
     * @param string $domain
     * @param string $fiscalName
     * @param string $requirementNumberYear
     */
    public function __construct(string $brand, string $document, string $domain, string $fiscalName, string $requirementNumberYear) {
        $this->brand = $brand;
        $this->document = $document;
        $this->domain = $domain;
        $this->fiscalName = $fiscalName;
        $this->requirementNumberYear = $requirementNumberYear;
    }

    /**
     * The brand name associated with the requirement.
     *
     * @param string $value
     * @throws Exception
     * @return string
     */
    public static function fromBrand(string $value): string {
        return $value; /*string*/
    }

    /**
     * The brand name associated with the requirement.
     *
     * @throws Exception
     * @return string
     */
    public function toBrand(): string {
        if (Bet::validateBrand($this->brand))  {
            return $this->brand; /*string*/
        }
        throw new Exception('never get to this Bet::brand');
    }

    /**
     * The brand name associated with the requirement.
     *
     * @param string
     * @return bool
     * @throws Exception
     */
    public static function validateBrand(string $value): bool {
        if (!is_string($value)) {
            throw new Exception("Attribute Error:Bet::brand");
        }
        return true;
    }

    /**
     * The brand name associated with the requirement.
     *
     * @throws Exception
     * @return string
     */
    public function getBrand(): string {
        if (Bet::validateBrand($this->brand))  {
            return $this->brand;
        }
        throw new Exception('never get to getBrand Bet::brand');
    }

    /**
     * The brand name associated with the requirement.
     *
     * @return string
     */
    public static function sampleBrand(): string {
        return 'Bet::brand::31'; /*31:brand*/
    }

    /**
     * The CNPJ document number of the company.
     *
     * @param string $value
     * @throws Exception
     * @return string
     */
    public static function fromDocument(string $value): string {
        return $value; /*string*/
    }

    /**
     * The CNPJ document number of the company.
     *
     * @throws Exception
     * @return string
     */
    public function toDocument(): string {
        if (Bet::validateDocument($this->document))  {
            return $this->document; /*string*/
        }
        throw new Exception('never get to this Bet::document');
    }

    /**
     * The CNPJ document number of the company.
     *
     * @param string
     * @return bool
     * @throws Exception
     */
    public static function validateDocument(string $value): bool {
        if (!is_string($value)) {
            throw new Exception("Attribute Error:Bet::document");
        }
        return true;
    }

    /**
     * The CNPJ document number of the company.
     *
     * @throws Exception
     * @return string
     */
    public function getDocument(): string {
        if (Bet::validateDocument($this->document))  {
            return $this->document;
        }
        throw new Exception('never get to getDocument Bet::document');
    }

    /**
     * The CNPJ document number of the company.
     *
     * @return string
     */
    public static function sampleDocument(): string {
        return 'Bet::document::32'; /*32:document*/
    }

    /**
     * The domain name associated with the brand.
     *
     * @param string $value
     * @throws Exception
     * @return string
     */
    public static function fromDomain(string $value): string {
        return $value; /*string*/
    }

    /**
     * The domain name associated with the brand.
     *
     * @throws Exception
     * @return string
     */
    public function toDomain(): string {
        if (Bet::validateDomain($this->domain))  {
            return $this->domain; /*string*/
        }
        throw new Exception('never get to this Bet::domain');
    }

    /**
     * The domain name associated with the brand.
     *
     * @param string
     * @return bool
     * @throws Exception
     */
    public static function validateDomain(string $value): bool {
        if (!is_string($value)) {
            throw new Exception("Attribute Error:Bet::domain");
        }
        return true;
    }

    /**
     * The domain name associated with the brand.
     *
     * @throws Exception
     * @return string
     */
    public function getDomain(): string {
        if (Bet::validateDomain($this->domain))  {
            return $this->domain;
        }
        throw new Exception('never get to getDomain Bet::domain');
    }

    /**
     * The domain name associated with the brand.
     *
     * @return string
     */
    public static function sampleDomain(): string {
        return 'Bet::domain::33'; /*33:domain*/
    }

    /**
     * The legal name of the company.
     *
     * @param string $value
     * @throws Exception
     * @return string
     */
    public static function fromFiscalName(string $value): string {
        return $value; /*string*/
    }

    /**
     * The legal name of the company.
     *
     * @throws Exception
     * @return string
     */
    public function toFiscalName(): string {
        if (Bet::validateFiscalName($this->fiscalName))  {
            return $this->fiscalName; /*string*/
        }
        throw new Exception('never get to this Bet::fiscalName');
    }

    /**
     * The legal name of the company.
     *
     * @param string
     * @return bool
     * @throws Exception
     */
    public static function validateFiscalName(string $value): bool {
        if (!is_string($value)) {
            throw new Exception("Attribute Error:Bet::fiscalName");
        }
        return true;
    }

    /**
     * The legal name of the company.
     *
     * @throws Exception
     * @return string
     */
    public function getFiscalName(): string {
        if (Bet::validateFiscalName($this->fiscalName))  {
            return $this->fiscalName;
        }
        throw new Exception('never get to getFiscalName Bet::fiscalName');
    }

    /**
     * The legal name of the company.
     *
     * @return string
     */
    public static function sampleFiscalName(): string {
        return 'Bet::fiscalName::34'; /*34:fiscalName*/
    }

    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     *
     * @param string $value
     * @throws Exception
     * @return string
     */
    public static function fromRequirementNumberYear(string $value): string {
        return $value; /*string*/
    }

    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     *
     * @throws Exception
     * @return string
     */
    public function toRequirementNumberYear(): string {
        if (Bet::validateRequirementNumberYear($this->requirementNumberYear))  {
            return $this->requirementNumberYear; /*string*/
        }
        throw new Exception('never get to this Bet::requirementNumberYear');
    }

    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     *
     * @param string
     * @return bool
     * @throws Exception
     */
    public static function validateRequirementNumberYear(string $value): bool {
        if (!is_string($value)) {
            throw new Exception("Attribute Error:Bet::requirementNumberYear");
        }
        return true;
    }

    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     *
     * @throws Exception
     * @return string
     */
    public function getRequirementNumberYear(): string {
        if (Bet::validateRequirementNumberYear($this->requirementNumberYear))  {
            return $this->requirementNumberYear;
        }
        throw new Exception('never get to getRequirementNumberYear Bet::requirementNumberYear');
    }

    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     *
     * @return string
     */
    public static function sampleRequirementNumberYear(): string {
        return 'Bet::requirementNumberYear::35'; /*35:requirementNumberYear*/
    }

    /**
     * @throws Exception
     * @return bool
     */
    public function validate(): bool {
        return Bet::validateBrand($this->brand)
        || Bet::validateDocument($this->document)
        || Bet::validateDomain($this->domain)
        || Bet::validateFiscalName($this->fiscalName)
        || Bet::validateRequirementNumberYear($this->requirementNumberYear);
    }

    /**
     * @return stdClass
     * @throws Exception
     */
    public function to(): stdClass  {
        $out = new stdClass();
        $out->{'brand'} = $this->toBrand();
        $out->{'document'} = $this->toDocument();
        $out->{'domain'} = $this->toDomain();
        $out->{'fiscal_name'} = $this->toFiscalName();
        $out->{'requirement_number_year'} = $this->toRequirementNumberYear();
        return $out;
    }

    /**
     * @param stdClass $obj
     * @return Bet
     * @throws Exception
     */
    public static function from(stdClass $obj): Bet {
        return new Bet(
         Bet::fromBrand($obj->{'brand'})
        ,Bet::fromDocument($obj->{'document'})
        ,Bet::fromDomain($obj->{'domain'})
        ,Bet::fromFiscalName($obj->{'fiscal_name'})
        ,Bet::fromRequirementNumberYear($obj->{'requirement_number_year'})
        );
    }

    /**
     * @return Bet
     */
    public static function sample(): Bet {
        return new Bet(
         Bet::sampleBrand()
        ,Bet::sampleDocument()
        ,Bet::sampleDomain()
        ,Bet::sampleFiscalName()
        ,Bet::sampleRequirementNumberYear()
        );
    }
}
