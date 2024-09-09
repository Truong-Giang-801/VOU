import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class AddUserPage extends StatefulWidget {
  @override
  _AddUserPageState createState() => _AddUserPageState();
}

class _AddUserPageState extends State<AddUserPage> {
  final _formKey = GlobalKey<FormState>();
  String? _username;
  String? _email;
  String? _phoneNumber;
  String? _password;
  String? _role;

  List<String> roles = ['ADMIN', 'BRAND', 'PLAYER'];
  bool _isLoading = false;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Thêm tài khoản mới'),
        backgroundColor: Colors.cyan,
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                decoration: InputDecoration(labelText: 'Tên người dùng'),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Vui lòng nhập tên người dùng';
                  }
                  return null;
                },
                onSaved: (value) => _username = value,
              ),
              TextFormField(
                decoration: InputDecoration(labelText: 'Email'),
                keyboardType: TextInputType.emailAddress,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Vui lòng nhập email';
                  } else if (!value.contains('@')) {
                    return 'Địa chỉ email không hợp lệ';
                  }
                  return null;
                },
                onSaved: (value) => _email = value,
              ),
              TextFormField(
                decoration: InputDecoration(labelText: 'Số điện thoại'),
                keyboardType: TextInputType.phone,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Vui lòng nhập số điện thoại';
                  }
                  return null;
                },
                onSaved: (value) => _phoneNumber = value,
              ),
              TextFormField(
                decoration: InputDecoration(labelText: 'Mật khẩu'),
                obscureText: true,
                validator: (value) {
                  if (value == null || value.length < 8) {
                    return 'Mật khẩu phải có ít nhất 8 ký tự';
                  }
                  return null;
                },
                onSaved: (value) => _password = value,
              ),
              DropdownButtonFormField<String>(
                decoration: InputDecoration(labelText: 'Quyền hạn'),
                items: roles.map((role) {
                  return DropdownMenuItem(
                    child: Text(role),
                    value: role,
                  );
                }).toList(),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Vui lòng chọn quyền hạn';
                  }
                  return null;
                },
                onChanged: (String? newValue) {
                  setState(() {
                    _role = newValue;
                  });
                },
                onSaved: (value) => _role = value,
              ),
              SizedBox(height: 16),
              ElevatedButton(
                onPressed: () async {
                  if (_formKey.currentState!.validate()) {
                    _formKey.currentState!.save();
                    setState(() {
                      _isLoading = true;
                    });
                    bool success = await addUser(_username!, _email!, _phoneNumber!, _password!, _role!);
                    setState(() {
                      _isLoading = false;
                    });
                    if (success) {
                      Navigator.of(context).pop(true); // Pass true to indicate success
                    }
                  }
                },
                child: _isLoading ? CircularProgressIndicator() : Text('Thêm tài khoản'),
              ),
            ],
          ),
        ),
      ),
    );
  }
  
  Future<bool> addUser(String username, String email, String phoneNumber, String password, String roleName) async {
    try {
      final Map<String, dynamic> userData = {
        'UserName': username,
        'Email': email,
        'PhoneNumber': phoneNumber,
        'Password': password,
        'RoleName': roleName,
      };

      final response = await http.post(
        Uri.parse('https://localhost:7001/api/auth/adduser'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode(userData),
      );

      // Print the response body
      print('Response status: ${response.statusCode}');
      print('Response body: ${response.body}');

      if (response.statusCode >= 200 && response.statusCode <= 299) {
        final responseData = jsonDecode(response.body);
        final userResult = responseData['result'];

        final user = {
          'id': userResult['id'],
          'name': userResult['name'], // Map to 'name' field
          'userName': userResult['userName'],
          'email': userResult['email'],
          'phoneNumber': userResult['phoneNumber'],
          'role': userResult['role'],
          'lockout': userResult['lockout'], // Ensure correct key
        };

        print('Created user: $user');

        ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Tài khoản đã được thêm thành công')));
        return true; // Indicate success
      } else if (response.statusCode == 400) {
        final errorData = jsonDecode(response.body);
        final errors = errorData['Result'] as List;
        ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Lỗi xác thực: ${errors.join(", ")}')));
      } else {
        ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Lỗi khi thêm tài khoản: ${response.body}')));
      }
    } catch (error) {
      print('Error adding user: $error');
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Có lỗi xảy ra khi thêm tài khoản')));
    }
    return false; // Indicate failure
  }
}
