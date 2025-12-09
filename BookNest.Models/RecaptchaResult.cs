using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class RecaptchaResult
{
	public bool success { get; set; }

	[JsonPropertyName("challenge_ts")]
	public DateTime challenge_ts { get; set; }

	public string hostname { get; set; }

	[JsonPropertyName("error-codes")]
	public List<string> error_codes { get; set; }
}