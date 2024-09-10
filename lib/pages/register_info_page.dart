import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'event_manager_page.dart'; // Import the EventManagerPage

class RegisterInfoPage extends StatefulWidget {
  final String userId; // Expect userId here

  RegisterInfoPage({required this.userId});

  @override
  _RegisterInfoPageState createState() => _RegisterInfoPageState();
}

class _RegisterInfoPageState extends State<RegisterInfoPage> {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();
  final _gpsController = TextEditingController();
  final _industryController = TextEditingController();
  final _addressController = TextEditingController();

  Future<int?> _registerBrand() async {
    final url = 'https://localhost:7002/api/brand'; // Replace with your API endpoint

    final response = await http.post(
      Uri.parse(url),
      headers: {
        'Content-Type': 'application/json',
      },
      body: json.encode({
        'Name': _nameController.text,
        'GPS': _gpsController.text,
        'Industry': _industryController.text,
        'Address': _addressController.text,
      }),
    );

    final responseData = json.decode(response.body);

    // Check the structure of the responseData
    print('RegisterBrand Response: $responseData');

    if (response.statusCode == 200 && responseData['isSuccess']) {
      // Ensure the 'id' field is present and valid
      return responseData['result']['id'];
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to Register Brand Information: ${responseData['message']}')),
      );
      return null;
    }
  }

  Future<void> _assignUserToBrand(int brandId) async {
    final url = 'https://localhost:7001/api/auth/user-brand'; // Replace with your API endpoint

    try {
      final response = await http.post(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
        },
        body: json.encode({
          'brandId': brandId,
          'userID': widget.userId,
        }),
      );

      print('Response status: ${response.statusCode}');
      print('Response body: ${response.body}');

      final responseData = json.decode(response.body);

      if (response.statusCode == 200) {
        if (responseData['isSuccess']) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('User successfully assigned to brand')),
          );

          // Navigate to EventManagerPage after successful assignment
          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (context) => EventManagerPage(
                userId: widget.userId,
              ),
            ),
          );
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Failed to assign user to brand: ${responseData['message']}')),
          );
        }
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error assigning user to brand')),
        );
      }
    } catch (error) {
      print('Error: $error');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Unexpected error occurred: $error')),
      );
    }
  }

  Future<void> _submitForm() async {
    if (_formKey.currentState?.validate() ?? false) {
      try {
        // Attempt to register the brand
        final brandId = await _registerBrand();
        print('Registered brand ID: $brandId'); // Debug print

        if (brandId != null) {
          // Assign user to the registered brand
          await _assignUserToBrand(brandId);
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Failed to get a valid brand ID')),
          );
        }
      } catch (error) {
        // Handle unexpected errors
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('An unexpected error occurred: $error')),
        );
      }
    } else {
      // If form validation fails
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Please fix the errors in the form')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Register Brand Information',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
            fontSize: 24,
          ),
        ),
        backgroundColor: Colors.cyan, // Dark blue background
        foregroundColor: Colors.white, // White text
        centerTitle: true, // Center-align the title
        elevation: 8, // Add shadow effect
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                controller: _nameController,
                decoration: InputDecoration(labelText: 'Name'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter a name';
                  }
                  return null;
                },
              ),
              TextFormField(
                controller: _gpsController,
                decoration: InputDecoration(labelText: 'GPS Coordinates'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter GPS coordinates';
                  }
                  return null;
                },
              ),
              TextFormField(
                controller: _industryController,
                decoration: InputDecoration(labelText: 'Industry'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter the industry';
                  }
                  return null;
                },
              ),
              TextFormField(
                controller: _addressController,
                decoration: InputDecoration(labelText: 'Address'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter an address';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20),
              ElevatedButton(
                onPressed: _submitForm,
                child: Text('Submit'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
