import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class RegisterInfoPage extends StatefulWidget {
  final int brandId; // Pass the brand ID here

  RegisterInfoPage({required this.brandId});

  @override
  _RegisterInfoPageState createState() => _RegisterInfoPageState();
}

class _RegisterInfoPageState extends State<RegisterInfoPage> {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();
  final _gpsController = TextEditingController();
  final _industryController = TextEditingController();
  final _addressController = TextEditingController();

  Future<void> _submitForm() async {
    if (_formKey.currentState?.validate() ?? false) {
      final name = _nameController.text;
      final gps = _gpsController.text;
      final industry = _industryController.text;
      final address = _addressController.text;

      final url = 'https://localhost:7002/api/brand'; // Replace with your API endpoint

      final response = await http.post(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
        },
        body: json.encode({
          'Id': widget.brandId, // Include the brand ID
          'Name': name,
          'GPS': gps,
          'Industry': industry,
          'Address': address, // Include address if your API expects it
        }),
      );

      final responseData = json.decode(response.body);

      if (response.statusCode == 200 && responseData['isSuccess']) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Information Registered')),
        );
        // Navigate to another page or update the UI
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Failed to Register Information')),
        );
      }
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
