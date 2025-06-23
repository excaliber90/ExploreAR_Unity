using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, forgetPasswordPanel, notificationPanel;

    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupConfirmpassword, signupUsername, forgetPasswordEmail;
    public TMP_Text notif_Title_Text, notif_Message_Text;
    public Toggle rememberMe;

    FirebaseAuth auth;
    FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + task.Result);
            }
        });
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out: " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in: " + user.UserId);
            }
        }
    }

    public void OpenloginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetpasswordPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Field empty! Please fill it.");
            return;
        }

        SignInUser(loginEmail.text, loginPassword.text);
    }

    public void signUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupConfirmpassword.text))
        {
            showNotificationMessage("Error", "Field empty! Please fill it.");
            return;
        }

        CreateUser(signupEmail.text, signupPassword.text, signupUsername.text);
    }

    public void forgetPassword()
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                showNotificationMessage("Error", "Failed to send reset email.");
            }
            else
            {
                showNotificationMessage("Success", "Password reset email sent.");
            }
        });
    }

    private void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text = title;
        notif_Message_Text.text = message;
        notificationPanel.SetActive(true);
    }

    public void Closenotification()
    {
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";
        notificationPanel.SetActive(false);
    }

    public void logOut()
    {
        auth.SignOut();
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("email");
        OpenloginPanel();
    }

    void CreateUser(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("CreateUser failed: " + task.Exception);
                return;
            }

            user = task.Result.User;
            if (user == null)
            {
                Debug.LogError("User creation returned null.");
                return;
            }

            Debug.LogFormat("User created: {0} ({1})", user.DisplayName, user.UserId);
            UpdateUserProfile(username);
        });
    }

    void UpdateUserProfile(string username)
    {
        if (auth.CurrentUser != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = username,
                PhotoUrl = new Uri("https://placehold.co/600x400")
            };

            auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfile failed: " + task.Exception);
                    return;
                }

                // Save to PlayerPrefs
                PlayerPrefs.SetString("username", username);
                PlayerPrefs.SetString("email", auth.CurrentUser.Email);

                showNotificationMessage("Success", "Account Successfully Created!");

                // Optional: Load next scene here
                SceneManager.LoadScene("Main_Menu");
            });
        }
    }

   public void SignInUser(string email, string password)
{
    Debug.Log($"Attempting login with email: {email}");

    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
    {
        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Login error: " + task.Exception.Flatten().InnerException.Message);
            showNotificationMessage("Login Failed", "Invalid email or password.");
            return;
        }

        var signedInUser = task.Result.User;

        if (signedInUser != null)
        {
            Debug.Log("Login success. Reloading user...");

            signedInUser.ReloadAsync().ContinueWithOnMainThread(reloadTask =>
            {
                if (reloadTask.IsCompleted && !reloadTask.IsFaulted)
                {
                    Debug.Log("Reload complete. Username: " + signedInUser.DisplayName);
                    PlayerPrefs.SetString("username", signedInUser.DisplayName ?? "Guest");
                    PlayerPrefs.SetString("email", signedInUser.Email ?? "No Email");

                    SceneManager.LoadScene("Main_Menu");
                }
                else
                {
                    Debug.LogError("Failed to reload user: " + reloadTask.Exception);
                    showNotificationMessage("Error", "Failed to reload user data.");
                }
            });
        }
        else
        {
            Debug.LogError("Login failed: user is null");
        }
    });
}


    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }
}
