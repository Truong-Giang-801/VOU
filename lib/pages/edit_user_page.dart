import 'package:flutter/material.dart';
import 'package:vou_web/class/user.dart';
import 'package:vou_web/api_handler/user_api_handler.dart';
import 'dart:convert';

class EditUserPage extends StatefulWidget {
  final User user;

  EditUserPage({required this.user});

  @override
  _EditUserPageState createState() => _EditUserPageState();
}

class _EditUserPageState extends State<EditUserPage> {
  final _formKey = GlobalKey<FormState>();
  late TextEditingController _nameController;
  late TextEditingController _emailController;
  late TextEditingController _phoneNumberController;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: widget.user.name);
    _emailController = TextEditingController(text: widget.user.email ?? '');
    _phoneNumberController = TextEditingController(text: widget.user.phoneNumber ?? '');
  }

  @override
  void dispose() {
    _nameController.dispose();
    _emailController.dispose();
    _phoneNumberController.dispose();
    super.dispose();
  }

  void _saveUser() async {
    if (_formKey.currentState?.validate() ?? false) {
      final updatedUser = User(
        id: widget.user.id,
        name: _nameController.text,
        email: _emailController.text,
        phoneNumber: _phoneNumberController.text,
        password: widget.user.password, // Keep the existing password
        role: widget.user.role, // Keep the existing role
        lockout: widget.user.lockout,
      );

      // Logic to update the user without calling assignRole
      try {
        final response = await updateUser(user: updatedUser); // Assuming you have an updateUser function
        if (response.statusCode == 200) {
          Navigator.pop(context, true);
        } else {
          final errorMessage = json.decode(response.body)['message'] ?? 'Failed to update user';
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text(errorMessage)),
          );
        }
      } catch (error) {
        print('Error updating user: $error');
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('An error occurred')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Edit User'),
        backgroundColor: Colors.cyan,
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: ListView(
            children: [
              TextFormField(
                controller: _nameController,
                decoration: InputDecoration(labelText: 'Name'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter the name';
                  }
                  return null;
                },
              ),
              TextFormField(
                controller: _emailController,
                decoration: InputDecoration(labelText: 'Email'),
                keyboardType: TextInputType.emailAddress,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter the email';
                  }
                  return null;
                },
              ),
              TextFormField(
                controller: _phoneNumberController,
                decoration: InputDecoration(labelText: 'Phone Number'),
                keyboardType: TextInputType.phone,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter the phone number';
                  }
                  return null;
                },
              ),
              SizedBox(height: 20), // Add some space before the button
              ElevatedButton(
                onPressed: _saveUser,
                child: Text('Submit'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.cyan, // Set button color
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
