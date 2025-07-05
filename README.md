# KCSCommunity后端README

## 目前TODO:

- [x] 添加令牌刷新机制
- [x] 添加修改密码支持
- [x] 使Passcode可设置有效时段
- [ ] 添加Passkey支持

## 技术栈
| 类别             | 技术/库                                   | 用途与说明                                                                     |
| ---------------- | ----------------------------------------- | ------------------------------------------------------------------------------ |
| **框架与运行时** | .NET 8.0                                  |                                                        |
| **架构模式**     | 领域驱动设计 (DDD), CQRS                  | 指导项目分层与设计；通过`MediatR`库实现命令查询职责分离 |
| **数据库**       | PostgreSQL                                | 关系型数据库，善于全文查询，为后面论坛社区功能准备                                         |
| **ORM**          | Entity Framework Core 8                   |                    |
| **用户与认证**   | ASP.NET Core Identity                     | 提供用户管理、角色管理、密码策略等基础功能                                   |
| **密码哈希**     | Konscious.Security.Cryptography.Argon2    | 实现了 `IPasswordHasher`，使用当前最安全的 Argon2id 算法。                 |
| **鉴权机制**     | JWT (JSON Web Tokens)                     | 用于无状态的API鉴权，令牌短期有效，不设刷新机制以简化设计。                    |
| **API 文档**     | Swashbuckle.AspNetCore (Swagger)          | 自动生成交互式API文档，方便前端开发和API测试。（带swagger ui）                                 |
| **验证**         | FluentValidation                          |               |
| **对象映射**     | AutoMapper                                |                  |

## API 实现

*   **用户创建 (Admin-only)**:
    1.  仅管理员可通过 `POST /api/users/create` 创建用户。
    2.  系统自动为新用户生成一个临时的、安全的随机密码，并将其账户设置为锁定状态。
    3.  同时生成一个与用户关联的、有效期24小时的一次性通行证码 (`OneTimePasscode`)。
    4.  整个创建过程（创建用户、分配角色、生成通行证码）被包裹在一个数据库事务中，保证原子性。

*   **账户激活 (User)**:
    1.  新用户使用获取到的通行证码和用户名，调用 `POST /api/users/activate`。
    2.  激活时，用户需设置自己的新密码（有复杂度校验）。
    3.  系统验证通行证码的有效性，然后用新密码替换临时密码，并解除账户锁定。
    4.  为防止并发激活，此操作受资源锁（`IResourceLockService`）保护。

*   **登录与鉴权**:
    1.  用户使用用户名和密码调用 `POST /api/auth/login`。
    2.  系统使用 `Argon2id` 算法验证密码。
    3.  **密码哈希升级**: 如果验证时发现用户密码的哈希参数已过时，系统会在用户无感的情况下，自动用新参数重新哈希密码并更新存储，然后正常颁发令牌。
    4.  成功后返回一个短期有效的JWT。后续所有需要授权的请求都必须在 `Authorization` 头中携带此 `Bearer Token`。

*   **权限组管理 (Owner-only)**:
    *   通过 `/api/roles` 端点进行管理。
    *   **所有者（Owner）** 拥有最高权限，可以创建、删除自定义角色，并管理所有角色中的用户成员。
    *   为防止系统瘫痪，核心角色（Owner, Administrator, User）不可被删除。
    *   为防止权限丢失，禁止将最后一个用户从 Owner 角色中移除。
    *   角色成员更新操作受资源锁保护，防止并发修改冲突。

## API 安全机制

*   **JWT 鉴权**: 标准的 Bearer Token 认证，保护需要用户身份的端点。
*   **API 接口签名**:
    *   对所有非公开的API请求（除登录、激活、Swagger文档外），要求客户端进行HMAC-SHA256签名。
    *   客户端需在请求头中提供 `X-ApiKey`, `X-Timestamp`, `X-Nonce`, `X-Signature`。
    *   签名字符串构成: `HTTP方法 + URL全路径(含规范化排序后的QueryString) + Timestamp + Nonce + 请求体原文`
    *   防重放攻击: 服务器会缓存已使用的 `Nonce`，在设定的时间窗口内（默认300秒）拒绝重复的 Nonce。
    *   此机制可通过 `appsettings.json` 中的 `ApiSignature:Enabled` 开关进行全局启用或禁用（开发时可禁用以方便调试）。
