// To parse this data:
//
//   import { Convert } from "./file";
//
//   const bet = Convert.toBet(json);
//
// These functions will throw an error if the JSON doesn't
// match the expected interface, even if the JSON is valid.

/**
 * Schema to validate bets.
 */
export interface Bet {
    /**
     * The brand name associated with the requirement.
     */
    brand: string;
    /**
     * The CNPJ document number of the company.
     */
    document: string;
    /**
     * The domain name associated with the brand.
     */
    domain: string;
    /**
     * The legal name of the company.
     */
    fiscal_name: string;
    /**
     * The requirement identifier in the format 'NNNN/YYYY'.
     */
    requirement_number_year: string;
}

// Converts JSON strings to/from your types
// and asserts the results of JSON.parse at runtime
export class Convert {
    public static toBet(json: string): Bet[] {
        return cast(JSON.parse(json), a(r("Bet")));
    }

    public static betToJson(value: Bet[]): string {
        return JSON.stringify(uncast(value, a(r("Bet"))), null, 2);
    }
}

/**
 * Throws an error indicating that an invalid value has been encountered.
 * This function is intended to be used for validation purposes, providing
 * detailed information about the expected type and the actual value received.
 *
 * @param {any} typ - The expected type of the value, which will be converted
 *                    to a human-readable format for the error message.
 * @param {any} val - The actual value that was received, which is included
 *                    in the error message.
 * @param {any} key - The key associated with the value, if applicable.
 *                    This will be included in the error message to provide
 *                    context about which value is invalid.
 * @param {string} [parent=''] - An optional parent context, which can provide
 *                                additional information about where the error
 *                                occurred. This will also be included in the
 *                                error message.
 *
 * @throws {Error} Throws an error with a message detailing the expected type,
 *                 the actual value, and any relevant context (key and parent).
 *
 * @example
 * // Example usage:
 * invalidValue('string', 123, 'username', 'userProfile');
 * // This would throw an error: "Invalid value for key "username" on userProfile. Expected string but got 123"
 */
function invalidValue(typ: any, val: any, key: any, parent: any = ''): never {
    const prettyTyp = prettyTypeName(typ);
    const parentText = parent ? ` on ${parent}` : '';
    const keyText = key ? ` for key "${key}"` : '';
    throw Error(`Invalid value${keyText}${parentText}. Expected ${prettyTyp} but got ${JSON.stringify(val)}`);
}

/**
 * Generates a human-readable string representation of a given type.
 * This function handles various types including arrays, objects with literals,
 * and primitive types. It is particularly useful for creating descriptive
 * type names in documentation or error messages.
 *
 * @param {any} typ - The type to be converted into a string representation.
 * Can be of any type including arrays, objects, or primitives.
 *
 * @returns {string} A string that describes the type in a human-readable format.
 *
 * @example
 * // Example usage:
 * const typeName = prettyTypeName([undefined, 'string']);
 * console.log(typeName); // Output: "an optional string"
 *
 * @example
 * // Handling an array of types
 * const typeName = prettyTypeName(['string', 'number']);
 * console.log(typeName); // Output: "one of [string, number]"
 *
 * @example
 * // Handling an object with a literal
 * const typeName = prettyTypeName({ literal: 'CustomType' });
 * console.log(typeName); // Output: "CustomType"
 *
 * @throws {TypeError} Throws an error if the input type is not valid.
 */
function prettyTypeName(typ: any): string {
    if (Array.isArray(typ)) {
        if (typ.length === 2 && typ[0] === undefined) {
            return `an optional ${prettyTypeName(typ[1])}`;
        } else {
            return `one of [${typ.map(a => { return prettyTypeName(a); }).join(", ")}]`;
        }
    } else if (typeof typ === "object" && typ.literal !== undefined) {
        return typ.literal;
    } else {
        return typeof typ;
    }
}

/**
 * Converts a type definition object to a mapping of JSON properties to JavaScript properties.
 *
 * This function checks if the provided type object already has a `jsonToJS` mapping.
 * If not, it creates one by iterating over the properties defined in the `props` array
 * of the type object, mapping each JSON property name to its corresponding JavaScript
 * property name and type.
 *
 * @param {any} typ - The type definition object containing a `props` array with property mappings.
 * @returns {any} The mapping of JSON properties to JavaScript properties.
 *
 * @example
 * const typeDef = {
 *   props: [
 *     { json: 'firstName', js: 'firstName', typ: 'string' },
 *     { json: 'age', js: 'age', typ: 'number' }
 *   ]
 * };
 * const mapping = jsonToJSProps(typeDef);
 * console.log(mapping); // { firstName: { key: 'firstName', typ: 'string' }, age: { key: 'age', typ: 'number' } }
 *
 * @throws {TypeError} Throws an error if the `props` attribute is not an array.
 */
function jsonToJSProps(typ: any): any {
    if (typ.jsonToJS === undefined) {
        const map: any = {};
        typ.props.forEach((p: any) => map[p.json] = { key: p.js, typ: p.typ });
        typ.jsonToJS = map;
    }
    return typ.jsonToJS;
}

/**
 * Converts JavaScript object properties to JSON properties.
 * This function checks if the provided type has a `jsToJSON` mapping.
 * If not, it creates a mapping from JavaScript property names to JSON property names
 * based on the provided `props` array, and stores it in the `jsToJSON` field of the type.
 *
 * @param {any} typ - The type object containing properties to be mapped.
 * @returns {any} The mapping of JavaScript property names to JSON property names.
 *
 * @example
 * const type = {
 *   props: [
 *     { js: 'name', json: 'Name', typ: 'string' },
 *     { js: 'age', json: 'Age', typ: 'number' }
 *   ]
 * };
 * const jsonProps = jsToJSONProps(type);
 * // jsonProps will be:
 * // {
 * //   name: { key: 'Name', typ: 'string' },
 * //   age: { key: 'Age', typ: 'number' }
 * // }
 *
 * @throws {TypeError} Throws an error if `typ.props` is not an array.
 */
function jsToJSONProps(typ: any): any {
    if (typ.jsToJSON === undefined) {
        const map: any = {};
        typ.props.forEach((p: any) => map[p.js] = { key: p.json, typ: p.typ });
        typ.jsToJSON = map;
    }
    return typ.jsToJSON;
}

/**
 * Transforms a value to match a specified type, handling various type scenarios including primitives, unions, enums, arrays, dates, and objects.
 *
 * @param {any} val - The value to be transformed.
 * @param {any} typ - The type definition that the value should conform to.
 * @param {function} getProps - A function that retrieves properties for object types.
 * @param {string} [key=''] - An optional key for error reporting.
 * @param {string} [parent=''] - An optional parent context for error reporting.
 * @returns {any} The transformed value if it matches the specified type; otherwise, an error is thrown.
 *
 * @throws {Error} Throws an error if the value does not match the expected type.
 *
 * @example
 * // Example usage:
 * const result = transform(value, expectedType, getPropertiesFunction);
 */
function transform(val: any, typ: any, getProps: any, key: any = '', parent: any = ''): any {
    /**
     * Transforms a value based on its expected type.
     *
     * This function checks if the type of the provided value matches the expected type.
     * If they match, the value is returned as is. If they do not match, an error is raised
     * indicating that the value is invalid for the specified type.
     *
     * @param {string} typ - The expected type of the value.
     * @param {any} val - The value to be transformed.
     * @returns {any} The original value if its type matches the expected type; otherwise, an error is thrown.
     *
     * @throws {Error} Throws an error if the value does not match the expected type.
     *
     * @example
     * // Returns '42'
     * transformPrimitive('number', 42);
     *
     * // Throws an error
     * transformPrimitive('number', 'not a number');
     */
    function transformPrimitive(typ: string, val: any): any {
        if (typeof typ === typeof val) return val;
        return invalidValue(typ, val, key, parent);
    }

    /**
     * Transforms a value by validating it against an array of types.
     * The function attempts to transform the provided value using each type in the array until one succeeds.
     *
     * @param {any[]} typs - An array of types against which the value will be validated.
     * @param {any} val - The value to be transformed and validated.
     * @returns {any} - The transformed value if validation is successful; otherwise, an indication of an invalid value.
     *
     * @throws {Error} - Throws an error if the value does not match any of the provided types.
     *
     * @example
     * const types = [typeA, typeB, typeC];
     * const result = transformUnion(types, someValue);
     * // result will be the transformed value or an error if validation fails.
     */
    function transformUnion(typs: any[], val: any): any {
        // val must validate against one typ in typs
        const l = typs.length;
        for (let i = 0; i < l; i++) {
            const typ = typs[i];
            try {
                return transform(val, typ, getProps);
            } catch (_) {}
        }
        return invalidValue(typs, val, key, parent);
    }

    /**
     * Transforms a value based on a set of predefined cases.
     * If the provided value is found within the cases, it returns the value.
     * Otherwise, it processes the value as an invalid entry and returns an error.
     *
     * @param {string[]} cases - An array of valid case strings to check against.
     * @param {any} val - The value to be transformed or validated.
     * @returns {any} - Returns the original value if it is valid; otherwise, it returns an error response.
     *
     * @throws {Error} - Throws an error if the value is not found in the cases and cannot be processed.
     *
     * @example
     * const result = transformEnum(['case1', 'case2'], 'case1');
     * // result will be 'case1'
     *
     * const invalidResult = transformEnum(['case1', 'case2'], 'invalidCase');
     * // invalidResult will be an error response indicating 'invalidCase' is not valid.
     */
    function transformEnum(cases: string[], val: any): any {
        if (cases.indexOf(val) !== -1) return val;
        return invalidValue(cases.map(a => { return l(a); }), val, key, parent);
    }

    /**
     * Transforms an array of values based on a specified type.
     *
     * This function checks if the provided value is an array and ensures that all elements
     * within the array are valid according to the specified type. If the value is not an array,
     * it returns an error indicating the invalid value.
     *
     * @param {any} typ - The type to which each element of the array should be transformed.
     * @param {any} val - The value to be transformed, which must be an array.
     * @returns {any} - The transformed array if valid, or an error if the input is not an array.
     *
     * @throws {Error} Will throw an error if `val` is not an array.
     *
     * @example
     * const result = transformArray(SomeType, [1, 2, 3]);
     * // result will be the transformed array based on SomeType
     *
     * @example
     * const result = transformArray(SomeType, "not an array");
     * // result will indicate an invalid value error
     */
    function transformArray(typ: any, val: any): any {
        // val must be an array with no invalid elements
        if (!Array.isArray(val)) return invalidValue(l("array"), val, key, parent);
        return val.map(el => transform(el, typ, getProps));
    }

    /**
     * Transforms a given value into a Date object.
     *
     * This function takes an input value and attempts to convert it into a Date object.
     * If the input value is null, it returns null. If the conversion fails (i.e., the
     * resulting Date object is invalid), it calls the `invalidValue` function to handle
     * the error appropriately.
     *
     * @param {any} val - The value to be transformed into a Date. This can be any type,
     *                    including null or an invalid date string.
     * @returns {any} - Returns a Date object if the transformation is successful,
     *                  null if the input is null, or an error response from `invalidValue`
     *                  if the input cannot be converted to a valid Date.
     *
     * @throws {Error} - Throws an error if the input value is not a valid date and
     *                   cannot be processed by `invalidValue`.
     *
     * @example
     * // Valid date string
     * const date1 = transformDate("2023-10-01"); // Returns a Date object for October 1, 2023
     *
     * // Null input
     * const date2 = transformDate(null); // Returns null
     *
     * // Invalid date string
     * const date3 = transformDate("invalid-date"); // Returns an error response from `invalidValue`
     */
    function transformDate(val: any): any {
        if (val === null) {
            return null;
        }
        const d = new Date(val);
        if (isNaN(d.valueOf())) {
            return invalidValue(l("Date"), val, key, parent);
        }
        return d;
    }

    /**
     * Transforms an object based on specified properties and additional values.
     *
     * This function takes an object and maps its properties according to the provided
     * property definitions. It also handles additional properties that may not be defined
     * in the initial set of properties.
     *
     * @param {Object} props - An object defining the properties to transform.
     * @param {any} additional - Additional value to be used for transformation of extra properties.
     * @param {any} val - The object to be transformed.
     * @returns {any} The transformed object.
     *
     * @throws {Error} Throws an error if the provided value is null, not an object, or an array.
     *
     * @example
     * const props = { name: { key: 'fullName', typ: 'string' } };
     * const additional = { age: 'number' };
     * const val = { name: 'John Doe' };
     * const result = transformObject(props, additional, val);
     * // result will be { fullName: 'John Doe' }
     */
    function transformObject(props: { [k: string]: any }, additional: any, val: any): any {
        if (val === null || typeof val !== "object" || Array.isArray(val)) {
            return invalidValue(l(ref || "object"), val, key, parent);
        }
        const result: any = {};
        Object.getOwnPropertyNames(props).forEach(key => {
            const prop = props[key];
            const v = Object.prototype.hasOwnProperty.call(val, key) ? val[key] : undefined;
            result[prop.key] = transform(v, prop.typ, getProps, key, ref);
        });
        Object.getOwnPropertyNames(val).forEach(key => {
            if (!Object.prototype.hasOwnProperty.call(props, key)) {
                result[key] = transform(val[key], additional, getProps, key, ref);
            }
        });
        return result;
    }

    if (typ === "any") return val;
    if (typ === null) {
        if (val === null) return val;
        return invalidValue(typ, val, key, parent);
    }
    if (typ === false) return invalidValue(typ, val, key, parent);
    let ref: any = undefined;
    while (typeof typ === "object" && typ.ref !== undefined) {
        ref = typ.ref;
        typ = typeMap[typ.ref];
    }
    if (Array.isArray(typ)) return transformEnum(typ, val);
    if (typeof typ === "object") {
        return typ.hasOwnProperty("unionMembers") ? transformUnion(typ.unionMembers, val)
            : typ.hasOwnProperty("arrayItems")    ? transformArray(typ.arrayItems, val)
            : typ.hasOwnProperty("props")         ? transformObject(getProps(typ), typ.additional, val)
            : invalidValue(typ, val, key, parent);
    }
    // Numbers can be parsed by Date but shouldn't be.
    /**
     * Converts a JSON string representation of bets into an array of Bet objects.
     *
     * This method parses the provided JSON string and casts the resulting object
     * to an array of Bet instances. It is a static method and can be called
     * without creating an instance of the class.
     *
     * @param {string} json - The JSON string to be parsed, which should represent
     *                        an array of Bet objects.
     * @returns {Bet[]} An array of Bet objects created from the parsed JSON data.
     *
     * @throws {SyntaxError} Throws an error if the JSON string is not valid.
     * @throws {TypeError} Throws an error if the parsed data cannot be cast to
     *                     an array of Bet objects.
     *
     * @example
     * const jsonString = '[{"id": 1, "amount": 100}, {"id": 2, "amount": 200}]';
     * const bets = MyClass.toBet(jsonString);
     * console.log(bets); // Outputs an array of Bet objects
     */
    if (typ === Date && typeof val !== "number") return transformDate(val);
    return transformPrimitive(typ, val);
}

/**
 * Converts an array of Bet objects into a JSON string representation.
 *
 * This method takes an array of Bet instances, processes them to ensure they are correctly formatted,
 * and then serializes the result into a JSON string. The output is formatted with a 2-space indentation
 * for better readability.
 *
 * @param {Bet[]} value - An array of Bet objects to be converted to JSON.
 * @returns {string} A JSON string representation of the provided Bet array.
 *
 * @example
 * const bets = [new Bet(...), new Bet(...)];
 * const jsonString = betToJson(bets);
 * console.log(jsonString);
 *
 * @throws {Error} Throws an error if the input value is not an array of Bet objects.
 */
/**
 * Casts a value to a specified type using transformation rules.
 *
 * This function takes a value of any type and attempts to transform it
 * into the specified type. The transformation is performed using the
 * provided type information and a set of JSON to JavaScript property
 * mappings.
 *
 * @template T - The target type to which the value should be cast.
 * @param {any} val - The value to be cast to the specified type.
 * @param {any} typ - The type information that defines how to transform the value.
 * @returns {T} - The transformed value cast to the specified type.
 *
 * @throws {Error} - Throws an error if the transformation fails or if the value
 *                   cannot be cast to the specified type.
 *
 * @example
 * // Example usage:
 * const result = cast<MyType>(someValue, myTypeInfo);
 */
function cast<T>(val: any, typ: any): T {
    return transform(val, typ, jsonToJSProps);
}

/**
 * Transforms a value to a specified type using a transformation function.
 *
 * This function takes a value and a type definition, and applies a transformation
 * to the value based on the provided type. It is useful for converting data
 * structures or types in a type-safe manner.
 *
 * @template T - The type of the input value.
 * @param {T} val - The value to be transformed.
 * @param {any} typ - The type definition to which the value should be transformed.
 * @returns {any} - The transformed value.
 *
 * @example
 * const result = uncast(someValue, someTypeDefinition);
 *
 * @throws {Error} - Throws an error if the transformation fails.
 */
function uncast<T>(val: T, typ: any): any {
    return transform(val, typ, jsToJSONProps);
}

/**
 * Creates an object containing a literal representation of the provided type.
 *
 * This function takes a type as an argument and returns an object with a single property `literal`
 * that holds the value of the provided type. This can be useful for type manipulation or
 * transformation purposes within TypeScript.
 *
 * @param {any} typ - The type to be represented as a literal. This can be any valid JavaScript type.
 * @returns {{ literal: any }} An object containing the literal representation of the provided type.
 *
 * @example
 * const result = l(42);
 * console.log(result); // { literal: 42 }
 *
 * @example
 * const result = l("Hello, World!");
 * console.log(result); // { literal: "Hello, World!" }
 */
function l(typ: any) {
    return { literal: typ };
}

/**
 * Creates an object containing an array of items based on the provided type.
 *
 * This function takes a parameter of any type and returns an object
 * with a property `arrayItems` that holds the value of the input parameter.
 *
 * @param {any} typ - The input value to be stored in the `arrayItems` property.
 * @returns {{ arrayItems: any }} An object with a single property `arrayItems`
 *          containing the value of the input parameter.
 *
 * @example
 * const result = a([1, 2, 3]);
 * console.log(result); // { arrayItems: [1, 2, 3] }
 *
 * @example
 * const result = a('Hello');
 * console.log(result); // { arrayItems: 'Hello' }
 */
function a(typ: any) {
    return { arrayItems: typ };
}

/**
 * Creates an object representing a union of types.
 *
 * This function accepts a variable number of arguments, which are expected to be types,
 * and returns an object containing those types as union members.
 *
 * @param {...any[]} typs - The types to be included in the union.
 * @returns {{ unionMembers: any[] }} An object with a property `unionMembers` that holds the provided types.
 *
 * @example
 * const union = u(String, Number, Boolean);
 * console.log(union.unionMembers); // Output: [String, Number, Boolean]
 */
function u(...typs: any[]) {
    return { unionMembers: typs };
}

/**
 * Creates an object containing the provided properties and additional data.
 *
 * @param {any[]} props - An array of properties to be included in the object.
 * @param {any} additional - Additional data to be included in the object.
 * @returns {{ props: any[], additional: any }} An object containing the provided properties and additional data.
 *
 * @example
 * const result = o(['prop1', 'prop2'], { key: 'value' });
 * console.log(result);
 * // Output: { props: ['prop1', 'prop2'], additional: { key: 'value' } }
 */
function o(props: any[], additional: any) {
    return { props, additional };
}

/**
 * Creates an object containing properties and additional data.
 *
 * This function takes an additional parameter and returns an object
 * with a predefined structure. The returned object includes an empty
 * array for properties and the provided additional data.
 *
 * @param {any} additional - The additional data to be included in the returned object.
 * @returns {{ props: Array, additional: any }} An object containing an empty array of properties and the additional data.
 *
 * @example
 * const result = m({ key: 'value' });
 * console.log(result); // { props: [], additional: { key: 'value' } }
 */
function m(additional: any) {
    return { props: [], additional };
}

/**
 * Creates an object with a reference to the provided name.
 *
 * This function takes a string as input and returns an object containing
 * a single property `ref`, which holds the value of the input string.
 *
 * @param {string} name - The name to be referenced in the returned object.
 * @returns {{ ref: string }} An object containing the reference to the provided name.
 *
 * @example
 * const result = r('example');
 * console.log(result.ref); // Output: 'example'
 */
function r(name: string) {
    return { ref: name };
}

const typeMap: any = {
    "Bet": o([
        { json: "brand", js: "brand", typ: "" },
        { json: "document", js: "document", typ: "" },
        { json: "domain", js: "domain", typ: "" },
        { json: "fiscal_name", js: "fiscal_name", typ: "" },
        { json: "requirement_number_year", js: "requirement_number_year", typ: "" },
    ], false),
};
