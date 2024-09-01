Steps: 

1. **Clone the GitHub repository as a mirror:**
   ```bash
   git clone --mirror https://github.com/<github-username>/<repository-name>.git
   ```

2. **Change directory to the cloned repository:**
   ```bash
   cd <repository-name>.git
   ```

3. **Add the GitLab remote repository URL:**
   ```bash
   git remote set-url origin https://gitlab.com/<gitlab-username>/<new-repository-name>.git
   ```

4. **Push the repository to GitLab:**
   ```bash
   git push --mirror
   ```

### Example:
If your GitHub username is `johnDoe`, and the repository is named `sample-repo`, and you want to migrate it to GitLab under the same name:

```bash
git clone --mirror https://github.com/sugan0tech/fe-dump.git
cd sample-repo.git
git remote set-url origin https://gitlab.com/sugan0tech/fe-dump.git
git push --mirror # give the access token  as creds
```
